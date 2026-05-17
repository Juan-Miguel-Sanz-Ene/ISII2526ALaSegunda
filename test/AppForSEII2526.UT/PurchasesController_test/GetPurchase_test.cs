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

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchases_test : AppForSEII2526SqliteUT
    {
        public GetPurchases_test()
        {
            var brand = new Brand { Id = 1, Name = "Zara", Location = "Madrid" };
            var prod1 = new Product { ProductId = 1, Name = "Jacket", Colour = "Red", Price = 20.0m, Stock = 10, IsReturnable = true, Brand = brand };
            var prod2 = new Product { ProductId = 2, Name = "Shirt", Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brand };
            _context.AddRange(brand, prod1, prod2);

            var user = new ApplicationUser
            {
                Id = "U1",
                Name = "Pepe",
                Surname = "Pérez",
                Address = "C/ Prueba 1",
                AccountCreationDate = System.DateTime.UtcNow,
                UserName = "pepe@test.com",
                Email = "pepe@test.com"
            };
            var pm = new Bizum { User = user, TelephoneNumber = "+34600111222" };
            _context.AddRange(user, pm);
            _context.SaveChanges();

            var order = new PurchaseOrder
            {
                City = "Albacete",
                Street = "Calle Inventada",
                PostalCode = "02001",
                NameSurname = "Pepe Pérez",
                Date = System.DateTime.Now,
                State = PurchaseState.Request,
                Customer = user,
                PaymentMethodId = pm.Id,
                TotalPrice = 0m,
                Rating = 5 // <-- Adding the optional rating to the DB
            };
            _context.PurchaseOrders.Add(order);
            _context.SaveChanges();

            var line1 = new PurchaseProduct { PurchaseOrderId = order.Id, ProductId = prod1.ProductId, Price = prod1.Price, Quantity = 2 };
            var line2 = new PurchaseProduct { PurchaseOrderId = order.Id, ProductId = prod2.ProductId, Price = prod2.Price, Quantity = 1 };
            _context.PurchaseProducts.AddRange(line1, line2);
            _context.SaveChanges();

            order.TotalPrice = line1.Price * line1.Quantity + line2.Price * line2.Quantity;
            _context.PurchaseOrders.Update(order);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPurchase_Successful_Test()
        {
            //arrange
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            //act
            var result = await sut.GetPurchase(1);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPurchaseDTO = Assert.IsType<PurchaseForDetailDTO>(okResult.Value);

            var expectedItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(1, "Jacket", "Zara", "Red",  20.0m, 2),
                new PurchaseItemDTO(2, "Shirt",  "Zara", "Blue", 10.0m, 1)
            };

            var expectedPurchaseDTO = new PurchaseForDetailDTO(
                id:             1,
                totalPrice:     50.0m,
                date:           actualPurchaseDTO.Date,   // non-deterministic; taken from actual
                street:         "Calle Inventada",
                city:           "Albacete",
                postalCode:     "02001",
                nameSurname:    "Pepe Pérez",
                state:          "Request",
                paymentMethod:  "Bizum",
                customerUserName: "pepe@test.com",
                items:          expectedItems,
                rating:         5
            );

            Assert.Equal(expectedPurchaseDTO, actualPurchaseDTO);
        }

        [Fact]
        public async Task GetPurchase_NotFound_test()
        {
            //arrange
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            //act
            var result = await sut.GetPurchase(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}