using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
namespace AppForSEII2526.UT.PurchasesController_test
{
    public class PostPurchases_test : AppForSEII2526SqliteUT
    {
        private static int _pmId = 1;

        public PostPurchases_test()
        {
            var brand = new Brand { Id = 1, Name = "Zara", Location = "Madrid" };
            var p1 = new Product { ProductId = 1, Name = "Jacket", Colour = "Red", Price = 20.0m, Stock = 10, IsReturnable = true, Brand = brand };
            var p2 = new Product { ProductId = 2, Name = "Shirt", Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brand };
            _context.AddRange(brand, p1, p2);

            var user = new ApplicationUser
            {
                Id = "U1",
                Name = "Pepe",
                Surname = "Pérez",
                Address = "C/ X",
                AccountCreationDate = DateTime.UtcNow,
                UserName = "pepe@test.com",
                Email = "pepe@test.com"
            };
            var pm = new Bizum { Id = _pmId, User = user, TelephoneNumber = "+34600111222" };
            _context.AddRange(user, pm);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
            var purchaseNoItems = new PurchaseForCreateDTO("Calle 1", "City", "00000", "Pepe", "Pérez", new List<PurchaseItemDTO>(), _pmId, 5);

            var itemsDeviceNoExist = new List<PurchaseItemDTO> { new PurchaseItemDTO(9999, "Ghost", "Zara", "Red", 0m, 1) };
            var purchaseDeviceNoExist = new PurchaseForCreateDTO("Calle 1", "City", "00000", "Pepe", "Pérez", itemsDeviceNoExist, _pmId, 5);

            var itemsQuantityHigh = new List<PurchaseItemDTO> { new PurchaseItemDTO(1, "Jacket", "Zara", "Red", 0m, 11) };
            var purchaseQuantityTooHigh = new PurchaseForCreateDTO("Calle 1", "City", "00000", "Pepe", "Pérez", itemsQuantityHigh, _pmId, 5);

            List<object[]> testcases = new List<object[]>()
            {
                new object[] { purchaseDeviceNoExist, "Items" }, // AppForSEII2526 puts this error in the "Items" key
                new object[] { purchaseNoItems, "Items" },
                new object[] { purchaseQuantityTooHigh, "Stock" } // AppForSEII2526 puts this error in the "Stock" key
            };
            return testcases;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Test_Error(PurchaseForCreateDTO? purchaseforcreate, string errorExpectedKey)
        {
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            var result = await sut.CreatePurchase(purchaseforcreate);

            var badrequest = Assert.IsType<BadRequestObjectResult>(result);
            var problems = Assert.IsType<ValidationProblemDetails>(badrequest.Value);

            Assert.True(problems.Errors.ContainsKey(errorExpectedKey));
        }

        [Fact]
        public async Task CreatePurchase_Test_Successful()
        {
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            var items = new List<PurchaseItemDTO> { new PurchaseItemDTO(1, "Jacket", "Zara", "Red", 0m, 1) };
            var purchaseforcreate = new PurchaseForCreateDTO("Calle Inventada, 1", "Albacete", "02001", "Pepe", "Pérez", items, _pmId, 5);

            var result = await sut.CreatePurchase(purchaseforcreate);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDTO = Assert.IsType<PurchaseForDetailDTO>(created.Value);

            var expectedItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(1, "Jacket", "Zara", "Red", 20.0m, 1)
            };

            var expectedPurchaseDTO = new PurchaseForDetailDTO(
                id:               actualPurchaseDTO.Id,
                totalPrice:       20.0m,
                date:             actualPurchaseDTO.Date,
                street:           "Calle Inventada, 1",
                city:             "Albacete",
                postalCode:       "02001",
                nameSurname:      "Pepe Pérez",
                state:            "Request",
                paymentMethod:    "Bizum",
                customerUserName: "pepe@test.com",
                items:            expectedItems,
                rating:           5
            );

            Assert.Equal(expectedPurchaseDTO, actualPurchaseDTO);
        }
    }
}