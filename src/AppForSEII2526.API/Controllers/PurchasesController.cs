using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            var purchase = await _context.PurchaseOrders
                .Where(po => po.Id == id)

                .Include(po => po.PurchaseProducts)
                    .ThenInclude(pp => pp.Product)
                        .ThenInclude(p => p.Brand)

                .Include(po => po.Customer)
                .Include(po => po.PaymentMethod)

                .Select(po => new PurchaseForDetailDTO(
                    po.Id,
                    po.TotalPrice,
                    po.Date,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.NameSurname,
                    po.State.ToString(), // enum -> string
                    EF.Property<string>(po.PaymentMethod, "Discriminator"),
                    po.Customer.UserName!,
                    po.PurchaseProducts
                        .Select(pp => new PurchaseItemDTO(
                            pp.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Product.Colour,
                            pp.Price,
                            pp.Quantity
                        )).ToList(),
                    po.Rating
                ))
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }

            return Ok(purchase);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseForCreateDTO purchaseForCreate)
        {
            //Validaciones
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (purchaseForCreate.Items == null || purchaseForCreate.Items.Count == 0)
            {
                ModelState.AddModelError("Items", "Error! You must include at least one product to be purchased");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar cantidades y existencia de productos
            var invalidQty = purchaseForCreate.Items
                .Where(i => i.Quantity < 1)
                .Select(i => i.ProductId)
                .ToList();
            if (invalidQty.Any())
            {
                ModelState.AddModelError("Items", $"Error! Quantity must be >= 1 for products: {string.Join(", ", invalidQty)}");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var productIds = purchaseForCreate.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            var missing = productIds.Except(products.Select(p => p.ProductId)).ToList();
            if (missing.Any())
            {
                ModelState.AddModelError("Items", $"Error! Products not found: {string.Join(", ", missing)}");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            var requestedByProd = purchaseForCreate.Items
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));
            foreach (var p in products)
            {
                if (p.Stock < requestedByProd[p.ProductId])
                {
                    ModelState.AddModelError("Stock", $"Error! Not enough stock for product '{p.Name}'");
                }
            }
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // PaymentMethod y usuario (precondición: el cliente está logado)
            var paymentMethod = await _context.PaymentMethods
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm => pm.Id == purchaseForCreate.PaymentMethodId);

            if (paymentMethod == null || paymentMethod.User == null)
            {
                ModelState.AddModelError("PaymentMethodId", "Error! Payment method not found or not associated to a user");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Street separated by comma
           

            if (string.IsNullOrWhiteSpace(purchaseForCreate.Street) || ! purchaseForCreate.Street.Contains(','))
           
            {
                ModelState.AddModelError("PaymentMethodId", "Error!, You must include a comma to separate the street from the number of the house");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }



            // Cabecera del pedido
            var order = new PurchaseOrder
            {
                City = purchaseForCreate.City,
                Street = purchaseForCreate.Street,
                PostalCode = purchaseForCreate.PostalCode,
                NameSurname = $"{purchaseForCreate.NameCustomer} {purchaseForCreate.SurnameCustomer}",
                Date = DateTime.Now,
                State = PurchaseState.Request,
                Customer = paymentMethod.User,
                PaymentMethodId = paymentMethod.Id,
                TotalPrice = 0m,
                Rating=purchaseForCreate.Rating
            };

            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
            foreach (var it in purchaseForCreate.Items)
            {
                var p = products.First(x => x.ProductId == it.ProductId);
                p.Stock -= it.Quantity;

                _context.PurchaseProducts.Add(new PurchaseProduct
                {
                    PurchaseOrderId = order.Id,
                    ProductId = p.ProductId,
                    Price = p.Price,
                    Quantity = it.Quantity
                });

                order.TotalPrice += p.Price * it.Quantity;
            }

            _context.PurchaseOrders.Update(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.Message}");
                return Conflict("Error " + ex.Message);
            }

            var dtoOut = await _context.PurchaseOrders
                .Where(po => po.Id == order.Id)
                .Include(po => po.PurchaseProducts)
                    .ThenInclude(pp => pp.Product)
                        .ThenInclude(p => p.Brand)
                .Include(po => po.Customer)
                .Include(po => po.PaymentMethod)
                .Select(po => new PurchaseForDetailDTO(
                    po.Id,
                    po.TotalPrice,
                    po.Date,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.NameSurname,
                    po.State.ToString(),
                    EF.Property<string>(po.PaymentMethod, "Discriminator"),
                    po.Customer.UserName!,
                    po.PurchaseProducts
                        .Select(pp => new PurchaseItemDTO(
                            pp.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Product.Colour,
                            pp.Price,
                            pp.Quantity
                        )).ToList(),
                    po.Rating
                ))
                .FirstAsync();

            return CreatedAtAction(nameof(GetPurchase), new { id = order.Id }, dtoOut);
        }

    }
}