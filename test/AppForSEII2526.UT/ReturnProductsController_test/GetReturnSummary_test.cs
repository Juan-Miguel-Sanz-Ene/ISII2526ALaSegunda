using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReturnProductsDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.ReturnProductsController_test
{
    public class GetReturnSummary_test : AppForSEII2526SqliteUT
    {
        public GetReturnSummary_test()
        {
            // ============= SEED =============

            // 1) Usuario (igual estilo que en el otro test)
            var user1 = new ApplicationUser
            {
                Id = "1",
                UserName = "TheUser1",
                Name = "User1",
                Surname = "Test1",
                Address = "Test Street 1",
                PhoneNumber = "111111111",
                Email = "user1@test.com"
            };

            // 2) PaymentMethod (Paypal, usando inicializador)
            var payment = new PayPal
            {
                // Id lo genera EF
                User = user1,
                Email = "user1@test.com"
            };

            // 3) Brand
            var brand1 = new Brand
            {
                // Id lo genera EF
                Name = "Brand1",
                Location = "Location1"
            };

            // 4) Product
            var product1 = new Product
            {
                ProductId = 2,
                Name = "ProductCok",
                Description = "Second Product",
                Colour = "Red",
                Price = 50.0m,
                Stock = 20,
                IsReturnable = true,
                Brand = brand1
            };

            // 5) PurchaseOrder (necesaria para el PurchaseProduct)
            var po1 = new PurchaseOrder
            {
                // Id lo fijamos para claridad, pero EF podría generarlo
                Id = 1,
                City = "City1",
                Street = "Street1",
                PostalCode = "00000",
                NameSurname = "User1 Test1",
                Description = "Test purchase order",
                Date = DateTime.Today,
                Rating = 5,
                TotalPrice = 200.0m,
                State = PurchaseState.Done,
                Customer = user1,
                PaymentMethod = payment,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            // 6) PurchaseProduct
            var pp1 = new PurchaseProduct
            {
                PurchaseOrder = po1,
                Product = product1,
                Quantity = 4,
                Price = 50.0m
            };
            po1.PurchaseProducts.Add(pp1);

            // 7) ReturnPurchaseOrder
            var rpo1 = new ReturnPurchaseOrder
            {
                Id = 1, // este Id es el que usaremos en el GET
                Name = "RPO-1",
                Date = DateTime.Today,
                MoneyToReturn = 100.0,
                NewTotalPrice = 100.0m,
                TotalPrice = 200.0m,
                Rating = 4,
                PaymentMethod = payment,
                Customer = user1,
                ReturnProducts = new List<ReturnProduct>()
            };

            // 8) ReturnProduct (línea de devolución)
            var rp1 = new ReturnProduct
            {
                // Id lo genera EF o lo puedes fijar si quieres
                Quantity = 2,
                Reason = "Defective item",
                ReturnPurchaseOrder = rpo1,
                PurchaseProduct = pp1
            };
            rpo1.ReturnProducts.Add(rp1);

            // ============= GUARDAR EN CONTEXTO =============
            _context.Users.Add(user1);
            _context.PaymentMethods.Add(payment);
            _context.Brands.Add(brand1);
            _context.Products.Add(product1);
            _context.PurchaseOrders.Add(po1);
            _context.PurchaseProducts.Add(pp1);
            _context.Returns.Add(rpo1);
            _context.ReturnProducts.Add(rp1);

            _context.SaveChanges();
        }

        // ========= TEST 1: NOT FOUND =========

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReturnSummary_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReturnProductsController>>();
            ILogger<ReturnProductsController> logger = mock.Object;

            var controller = new ReturnProductsController(_context, logger);

            // Act
            var result = await controller.GetReturnSummary(0); // Id que no existe

            // Assert
            var notFoundObject = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Return not found.", notFoundObject.Value);
        }

        // ========= TEST 2: FOUND =========

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReturnSummary_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReturnProductsController>>();
            ILogger<ReturnProductsController> logger = mock.Object;

            var controller = new ReturnProductsController(_context, logger);

            var expected = new ReturnSummaryDTO(
                customerName: "User1",
                customerSurname: "Test1",
                customerAddress: "Test Street 1",
                customerPhoneNumber: "111111111",
                paymentMethod: "PayPal", // PaymentMethod.GetType().Name
                returnedProducts: new List<ReturnedProductInfoDTO>
                {
                    new ReturnedProductInfoDTO(
                        quantity: 2,
                        productName: "ProductCok",
                        brandName: "Brand1",
                        warehouseLocation: "Location1"
                    )
                }
            );

            // Act
            var result = await controller.GetReturnSummary(1); // Id del RPO sembrado

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReturnSummaryDTO>(okResult.Value);

            Assert.Equal(expected, actual);
        }
    }
}
