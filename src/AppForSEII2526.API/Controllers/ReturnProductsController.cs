using AppForSEII2526.API.DTOs.ReturnProductsDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ReturnProductsController> _logger;

        public ReturnProductsController(ApplicationDbContext context, ILogger<ReturnProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{

        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + "Error:"+ error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReturnSummaryDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReturnSummary(int returnPurchaseOrderId)
        {
            ReturnSummaryDTO? summary = await _context.Returns

                // JOIN
                .Include(rpo => rpo.Customer)
                .Include(rpo => rpo.ReturnProducts)
                    .ThenInclude(rp => rp.PurchaseProduct)
                        .ThenInclude(pp => pp.Product)
                            .ThenInclude(p => p.Brand)

                .Where(rpo => rpo.Id == returnPurchaseOrderId)

                .Select(rpo => new ReturnSummaryDTO(



                    // Customer
                    rpo.Customer.Name,
                    rpo.Customer.Surname,
                    rpo.Customer.Address,
                    rpo.Customer.PhoneNumber,
                    rpo.PaymentMethod.GetType().Name,

                    // Products list
                    rpo.ReturnProducts.Select(rp => new ReturnedProductInfoDTO(
                        rp.Quantity,
                        rp.PurchaseProduct.Product.Name,
                        rp.PurchaseProduct.Product.Brand.Name,
                        rp.PurchaseProduct.Product.Brand.Location
                       
                    )).ToList()
                ))

                .FirstOrDefaultAsync();

            if (summary == null)
                return NotFound("Return not found.");

            return Ok(summary);
        }


    }
}
