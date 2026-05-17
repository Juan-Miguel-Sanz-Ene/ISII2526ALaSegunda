using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.BanReportController_test
{
    public class CreateBanReport_test : AppForSEII2526SqliteUT
    {
        public CreateBanReport_test()
        {
            var user1 = new ApplicationUser
            {
                Id = "1",
                UserName = "pepe@test.com",
                Email = "pepe@test.com",
                Name = "Pepe",
                Surname = "Test",
                Address = "Fake Street 123",
                AccountCreationDate = DateTime.UtcNow,
                Complaints = new List<Complaint>(),
                ReturnOrders = new List<ReturnPurchaseOrder>(),
                ReportCustomers = new List<ReportCustomer>()
            };

            var user2 = new ApplicationUser
            {
                Id = "2",
                UserName = "ana@test.com",
                Email = "ana@test.com",
                Name = "Ana",
                Surname = "Test",
                Address = "Fake Street 456",
                AccountCreationDate = DateTime.UtcNow,
                Complaints = new List<Complaint>(),
                ReturnOrders = new List<ReturnPurchaseOrder>(),
                ReportCustomers = new List<ReportCustomer>()
            };

            _context.Users.AddRange(user1, user2);

           
            var type = new ComplaintType
            {
                ID = 1,
                Name = "General"
            };
            _context.ComplaintTypes.Add(type);

            
            _context.Complaints.Add(new Complaint
            {
                ID = 10,
                User = user1,
                Processed = false,
                Description = "Toxic behavior",
                ComplaintDate = DateTime.UtcNow,
                Type = type
            });

            _context.SaveChanges();
        }




        [Fact(DisplayName = "UC_BF – CreateBanReport Success")]
        [Trait("UseCase", "UC_BF")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task UC_BF_CreateBanReport_Success_Test()
        {
            
            var dto = new BanReportForCreateDTO(
                reason: "Toxic behavior",
                detailedDescription: "Insults in chat",
                startDate: DateTime.UtcNow.AddDays(1),
                endDate: DateTime.UtcNow.AddDays(5),
                customers: new List<ReportCustomerForCreateDTO>
                {
                    new ReportCustomerForCreateDTO("1", "Repeated offenses")
                }
            );

            var mockLogger = new Mock<ILogger<BanReportController>>();
            var controller = new BanReportController(_context);

            
            var result = await controller.CreateBanReport(dto);

            
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<BanReportDTO>(ok.Value);

            Assert.Equal("Toxic behavior", response.Reason);
            Assert.Single(response.Customers);

            
            var reportInDb = _context.BanReports
                .Include(br => br.ReportCustomers)
                .FirstOrDefault(br => br.ID == response.Id);

            Assert.NotNull(reportInDb);
            Assert.Equal("1", reportInDb.ReportCustomers.First().CustomerId);

            
            Assert.True(_context.Complaints.First().Processed);
        }

       
        public static IEnumerable<object[]> ErrorCases()
        {
            return new List<object[]>
            {
               
                new object[]
                {
                    new BanReportForCreateDTO(
                        "Test",
                        "Desc",
                        DateTime.UtcNow.AddDays(1),
                        DateTime.UtcNow.AddDays(3),
                        new List<ReportCustomerForCreateDTO>
                        {
                            new ReportCustomerForCreateDTO("999", "X")
                        }
                    ),
                    "do not exist"
                },

                
                new object[]
                {
                    new BanReportForCreateDTO(
                        "Test",
                        "Desc",
                        DateTime.UtcNow.AddDays(1),
                        DateTime.UtcNow.AddDays(3),
                        new List<ReportCustomerForCreateDTO>
                        {
                            new ReportCustomerForCreateDTO("1", "X"),
                            new ReportCustomerForCreateDTO("1", "Y")
                        }
                    ),
                    "Duplicate"
                },

                
                new object[]
                {
                    new BanReportForCreateDTO(
                        "Test",
                        "Desc",
                        DateTime.UtcNow.AddDays(5),
                        DateTime.UtcNow.AddDays(1),
                        new List<ReportCustomerForCreateDTO>
                        {
                            new ReportCustomerForCreateDTO("1", "X")
                        }
                    ),
                    "earlier"
                }
            };
        }

        [Theory(DisplayName = "UC_AF – CreateBanReport Errors")]
        [Trait("UseCase", "UC_AF")]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(ErrorCases))]
        public async Task UC_AF_CreateBanReport_Error_Test(
            BanReportForCreateDTO dto,
            string expectedError)
        {
           
            var controller = new BanReportController(_context);

           
            var result = await controller.CreateBanReport(dto);

          
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var message = badRequest.Value.ToString();

            Assert.Contains(expectedError, message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
