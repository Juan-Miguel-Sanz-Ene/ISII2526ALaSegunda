using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;

namespace AppForSEII2526.UT.ProductsController_test
{
    public class GetProductsForPurchasing_test : AppForSEII2526SqliteUT
    {
        public GetProductsForPurchasing_test()
        {
            var brands = new List<Brand>() {
                new Brand { Id = 1, Name = "Zara",   Location = "Madrid"    },
                new Brand { Id = 2, Name = "Uniqlo", Location = "Barcelona" }
            };

            var products = new List<Product>(){
                new Product { ProductId = 1, Name = "Jacket", Colour = "Red",  Price = 20.0m, Stock = 4,  IsReturnable = true,  Brand = brands[0] },
                new Product { ProductId = 2, Name = "Shirt",  Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brands[1] },

                // this product has stock=0 so it shouldn't be returned when calling GetProductsForPurchasing
                new Product { ProductId = 3, Name = "Hat",    Colour = "Red",  Price =  5.0m, Stock = 0,  IsReturnable = false, Brand = brands[1] },
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchasing_returns_all_instock_when_no_filters()
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            var result = await controller.GetProductsForPurchasing(null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);

            var expected = new List<ProductForPurchaseDTO>
            {
                // controller orders by Colour (Blue then Red)
                new ProductForPurchaseDTO(2, "Shirt",  "Blue", "Uniqlo", 10, null, 10.0m),
                new ProductForPurchaseDTO(1, "Jacket", "Red",  "Zara",    4, null, 20.0m)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchasing_filters_by_colour()
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            var result = await controller.GetProductsForPurchasing("Red", null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);

            var expected = new List<ProductForPurchaseDTO>
            {
                new ProductForPurchaseDTO(1, "Jacket", "Red", "Zara", 4, null, 20.0m)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchasing_filters_by_name_substring()
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            var result = await controller.GetProductsForPurchasing(null, "irt");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);

            var expected = new List<ProductForPurchaseDTO>
            {
                new ProductForPurchaseDTO(2, "Shirt", "Blue", "Uniqlo", 10, null, 10.0m)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchasing_notfound_when_no_instock_matches()
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            // name="Hat" matches a product with Stock=0 → should be filtered out and return NotFound
            var result = await controller.GetProductsForPurchasing(null, "Hat");

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFound.Value);
            Assert.False(string.IsNullOrWhiteSpace(message));
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductsForPurchasing_filter_by_both_colour_and_name()
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            var result = await controller.GetProductsForPurchasing("Blue", "irt");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);

            var expected = new List<ProductForPurchaseDTO>
            {
                new ProductForPurchaseDTO(2, "Shirt", "Blue", "Uniqlo", 10, null, 10.0m)
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void GetProductsForPurchasing_Attributes_test()
        {
            var method = typeof(ProductsController).GetMethod(nameof(ProductsController.GetProductsForPurchasing));
            Assert.NotNull(method);

            var httpGetAttr = method!.GetCustomAttributes(typeof(HttpGetAttribute), false).FirstOrDefault();
            Assert.NotNull(httpGetAttr);

            var routeAttr = method.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().FirstOrDefault();
            Assert.NotNull(routeAttr);
            Assert.Equal("[action]", routeAttr!.Template);

            var produces = method.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), false)
                                  .Cast<ProducesResponseTypeAttribute>()
                                  .ToList();
            Assert.Contains(produces, a => a.StatusCode == StatusCodes.Status200OK && a.Type == typeof(IList<ProductForPurchaseDTO>));
        }
    }
}
