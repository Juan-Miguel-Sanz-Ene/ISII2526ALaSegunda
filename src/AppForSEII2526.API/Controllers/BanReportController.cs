using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class BanReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BanReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateBanReport")]
        public async Task<IActionResult> CreateBanReport([FromBody] BanReportForCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Customers == null || !dto.Customers.Any())
                return BadRequest("At least one customer must be included.");

            var duplicatedIds = dto.Customers
                .GroupBy(c => c.CustomerId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatedIds.Any())
                return BadRequest($"Duplicate CustomerId detected: {string.Join(", ", duplicatedIds)}");

           
            var customerIds = dto.Customers.Select(c => c.CustomerId).ToList();


            var existingCustomers = await _context.Users
            .Where(u => customerIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync();

            var missingCustomers = customerIds.Except(existingCustomers).ToList();

            if (missingCustomers.Any())
                return BadRequest($"These CustomerIds do not exist: {string.Join(", ", missingCustomers)}");

            var activeBans = await _context.BanReports
                .Include(br => br.ReportCustomers)
                .Where(br => br.State == ReportState.InProgress && br.EndDate > DateTime.UtcNow)
                .ToListAsync();

            var customersWithActiveBan = activeBans
                .SelectMany(br => br.ReportCustomers)
                .Where(rc => customerIds.Contains(rc.CustomerId))
                .Select(rc => rc.CustomerId)
                .Distinct()
                .ToList();

            if (customersWithActiveBan.Any())
                return BadRequest($"These customers already have an active ban: {string.Join(", ", customersWithActiveBan)}");

            var overlappingBans = await _context.BanReports
            .Include(br => br.ReportCustomers)
            .Where(br =>
                        customerIds.Any(id => br.ReportCustomers.Any(rc => rc.CustomerId == id)) &&
                        dto.StartDate < br.EndDate &&
                        dto.EndDate > br.StartDate)
            .ToListAsync();

            if (overlappingBans.Any())
                return BadRequest("There are overlapping bans for one or more customers.");

            if (dto.StartDate == null || dto.EndDate == null)
                return BadRequest("StartDate and EndDate are required.");

            
            if (dto.StartDate >= dto.EndDate)
                return BadRequest("StartDate must be earlier than EndDate.");

            
            if (dto.EndDate <= DateTime.UtcNow)
                return BadRequest("EndDate must be in the future.");



            var banReport = new BanReport
            {
                Reason = dto.Reason,
                DetailedDescription = dto.DetailedDescription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                State = ReportState.InProgress,
                ReportCustomers = new List<ReportCustomer>()
            };


            foreach (var dtoCustomer in dto.Customers)
            {
                var customerEntity = await _context.Users.FindAsync(dtoCustomer.CustomerId);

                if (customerEntity == null)
                    return BadRequest($"Customer {dtoCustomer.CustomerId} does not exist.");

                banReport.ReportCustomers.Add(new ReportCustomer
                {
                    CustomerId = dtoCustomer.CustomerId,
                    Customer = customerEntity,   
                    Message = dtoCustomer.Message,
                    State = ReportState.InProgress
                });
            }



            var complaintsToProcess = await _context.Complaints
                .Where(c => customerIds.Contains(c.User.Id) && !c.Processed)
                .ToListAsync();


            _context.BanReports.Add(banReport);
            await _context.SaveChangesAsync();

            
            foreach (var complaint in complaintsToProcess)
            {
                complaint.Processed = true;
                complaint.BanReportId = banReport.ID;
            }

           
            await _context.SaveChangesAsync();

            
            var result = new BanReportDTO
            {
                Id = banReport.ID,
                Reason = banReport.Reason,
                DetailedDescription = banReport.DetailedDescription,
                StartDate = banReport.StartDate,
                EndDate = banReport.EndDate,
                State = banReport.State.ToString(),
                Customers = banReport.ReportCustomers.Select(rc => new ReportCustomerDTO
                {
                    CustomerId = rc.CustomerId,
                    Message = rc.Message,
                    State = rc.State.ToString()
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("GetUsersForBan")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsersForBan()
        {
            var users = await _context.Users
                .Where(u => !_context.ReportCustomers
                    .Any(rc => rc.CustomerId == u.Id &&
                               rc.BanReport.State == ReportState.InProgress))
                .ToListAsync();

            return Ok(users);
        }




        [HttpGet("GetBanReport")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBanReport()
        {
            var reports = await _context.BanReports
                .Where(br => br.State == ReportState.InProgress)
                .Include(br => br.ReportCustomers)
                .ToListAsync();

            var result = reports.Select(br => new BanReportListDTO
            {
                Id = br.ID,
                Reason = br.Reason,
                DetailedDescription = br.DetailedDescription,
                StartDate = br.StartDate,
                EndDate = br.EndDate,
                State = "In progress",
                Customers = br.ReportCustomers.Select(rc => new ReportCustomerDTO
                {
                    CustomerId = rc.CustomerId,
                    Message = rc.Message,
                    State = rc.State.ToString()
                }).ToList()
            });

            return Ok(result);
        }

    }
}
