using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ComplaintsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComplaintsController> _logger;

        public ComplaintsController(ApplicationDbContext context, ILogger<ComplaintsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<ComplaintsResponseDTO>> GetPendingComplaints([FromQuery] ComplaintFilterDTO filter)
        {
            
            var query = _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Type)
                .Where(c => !c.Processed);

            
            if (!string.IsNullOrWhiteSpace(filter.Surname))
            {
                query = query.Where(c => c.User.Surname.Contains(filter.Surname));
            }

           
            if (!string.IsNullOrWhiteSpace(filter.ComplaintType))
            {
                query = query.Where(c => c.Type.Name == filter.ComplaintType);
            }

           
            var complaints = await query.ToListAsync();

            
            var groupedUsers = complaints
                .GroupBy(c => c.User)
                .Select(g => new UserComplaintsDTO
                {
                    Name = g.Key.Name,
                    Surname = g.Key.Surname,
                    AccountCreationDate = g.Key.AccountCreationDate,
                    ComplaintCount = g.Count(),
                    ComplaintTypes = g
                        .Select(c => c.Type.Name)
                        .Distinct()
                        .OrderBy(name => name)  
                        .ToList()
                })
                .OrderBy(u => u.Surname)
                .ThenBy(u => u.Name)
                .ToList();

           
            if (!groupedUsers.Any())
            {
                _logger.LogInformation("No users with complaints to be addressed.");

                return new ComplaintsResponseDTO
                {
                    HasComplaints = false,
                    Message = "No users with complaints to be addressed."
                };
            }

           
            return new ComplaintsResponseDTO
            {
                HasComplaints = true,
                Users = groupedUsers
            };
        }
    }
}
