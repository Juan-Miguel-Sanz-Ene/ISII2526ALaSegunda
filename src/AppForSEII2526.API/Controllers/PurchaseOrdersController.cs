using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseOrdersController> _logger;

        public PurchaseOrdersController(ApplicationDbContext context, ILogger<PurchaseOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        //public async Task<ActionResult>ComputeDivision(decimal op1, decimal op2)
        //{
        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now +" Error: "+ error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseOrderForDeliveryDTO>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetPurchaseOrdersToDelivery(string? postalcode, decimal? totalprice)
        {

            IList<PurchaseOrderForDeliveryDTO> purchaseOrdersDTOS = await _context.PurchaseOrders
                .Where(po => (po.State == PurchaseState.Request) &&
                             (po.TotalPrice >= totalprice || totalprice == null) &&
                             (po.PostalCode.Equals(postalcode) || postalcode == null) &&
                             (po.DriverAssigned == null))
                .Select(po => new PurchaseOrderForDeliveryDTO(po.Id, po.Date, po.TotalPrice, po.City, po.Street, po.PostalCode))
                .ToListAsync();
            if (purchaseOrdersDTOS.Count == 0)
            {
                string error = "No purchase orders in state 'Request' found with requested data";
                _logger.LogError(DateTime.Now + " Error: " + error);
                return BadRequest(error);
            }

            return Ok(purchaseOrdersDTOS);
        }
    }
}
