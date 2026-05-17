using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal),(int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

        //public async Task<ActionResult>computeDivide(decimal op1, decimal op2) {
        //    if (op2 == 0)
        //    {
        //        _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
        //        return BadRequest("op2 must be different from 0");
        //    }

        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ApplicationUserForBanningDTO>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersWithComplaints(string? surname, string? ComplaintType) {
            IList<ApplicationUserForBanningDTO> applicationUsers = await _context.Users
                    .Where(a =>

                    ((surname == null) || (a.Surname.Contains(surname)))
                    &&
                    a.Complaints.Any(
                        c => !c.Processed && (ComplaintType == null || c.Type.Name.Contains(ComplaintType))
                     )
                        )   

                .OrderBy(a=>a.UserName
                
                )
                .Select( c => new ApplicationUserForBanningDTO(
                    c.Id,
                    c.Name,
                    c.Surname,
                    c.AccountCreationDate,
                    c.Complaints
                    .Where( d => !d.Processed && (ComplaintType == null || d.Type.Name.Contains(ComplaintType))
                    )
                    .GroupBy( e => e.Type.Name)
                    .Select( g => new ComplaintDTO(
                        g.Key,
                        g.Count()))
                    .ToList()))
                .ToListAsync();


            if (applicationUsers == null) {
                _logger.LogError($"Error: User not found");
                return NotFound();
            }
            
            return Ok(applicationUsers);
        }
       

    }
}
   