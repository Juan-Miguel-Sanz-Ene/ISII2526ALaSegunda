using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.BanReportsController_test
{
    public class GetBanReport_test : AppForMovies4SqliteUT
    {
        public GetBanReport_test()
        {
           

            var user1 = new ApplicationUser
            {
                Id = "1",
                Name = "Juan",
                Surname = "García",
                Address = "Cardo Santo 2",
                AccountCreationDate = new DateTime(2025, 12, 12),
                UserName = "Juan1",
                EmailConfirmed = false
            };

            var user2 = new ApplicationUser
            {
                Id = "2",
                Name = "Jaime",
                Surname = "Sanchez",
                Address = "Calle inven",
                AccountCreationDate = new DateTime(2025, 11, 4),
                EmailConfirmed = true
            };

            var type1 = new ComplaintType { ID = 1, Name = "Paco" };
            var type3 = new ComplaintType { ID = 3, Name = "Antonio" };


            var complaints = new List<Complaint>
    {
        new Complaint
        {
            ID = 7,
            ComplaintDate = new DateTime(2025, 12, 2),
            Description = "Problema con el producto",
            Processed = false,
            User = user1,
            Type = type1
        },
        new Complaint
        {
            ID = 8,
            ComplaintDate = new DateTime(2025, 11, 4),
            Description = "askgflsdafhg",
            Processed = false,
            User = user2,
            Type = type3
        }
    };



            var banReport = new BanReport
            {
                ID = 1,
                Reason = "Harassment",
                DetailedDescription = "User repeatedly violated rules",
                StartDate = new DateTime(2025, 11, 14),
                EndDate = new DateTime(2025, 12, 14),
                ReportCustomers = new List<ReportCustomer>()
            };

            var reportCustomer = new ReportCustomer
            {
                BanReportId = banReport.ID,
                CustomerId = "1",
                State = ReportState.InProgress,
                Message = "Your account has been banned.",
                BanReport = banReport,
                Customer = user1
            };

            banReport.ReportCustomers.Add(reportCustomer);

           
            _context.Users.AddRange(user1, user2);
            _context.ComplaintTypes.AddRange(type1, type3);
            _context.Complaints.AddRange(complaints);
            _context.BanReports.Add(banReport);
            _context.ReportCustomers.Add(reportCustomer);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_GetBanReport()
        {
            var expected = new BanDetailDTO(
                1,
                "Harassment",
                "User repeatedly violated rules",
                new DateTime(2025, 11, 14),
                new DateTime(2025, 12, 14),
                "In progress",
                new List<ReportCustomerForDetailDTO>
                {
                    new ReportCustomerForDetailDTO("1", "Juan", "García", "Your account has been banned.")
                }
            );

            return new List<object[]>
            {
                new object[] { 1, expected },   
                new object[] { 999, null }      
            };
        }

        [Theory(DisplayName = "UC_BF_AF1 – GetBanReport")]
        [Trait("UseCase", "UC_BF_AF1")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_GetBanReport))]
        public async Task UC_BF_AF1_GetBanReport_Test(int reportId, BanDetailDTO expected)
        {
            
            var mockLogger = new Mock<ILogger<BanController>>();
            var controller = new BanController(_context, mockLogger.Object);

            var result = await controller.GetBanReport(reportId);

            if (expected == null)
            {
                
                Assert.IsType<NotFoundResult>(result);
            }
            else
            {
              
                var ok = Assert.IsType<OkObjectResult>(result);
                var actual = Assert.IsType<BanDetailDTO>(ok.Value);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Reason, actual.Reason);
                Assert.Equal(expected.DetailedDescription, actual.DetailedDescription);
                Assert.Equal(expected.StartDate, actual.StartDate);
                Assert.Equal(expected.EndDate, actual.EndDate);
                Assert.Equal(expected.State, actual.State);

                Assert.Single(actual.ReportedUsers);
                Assert.Equal(expected.ReportedUsers[0].CustomerId, actual.ReportedUsers[0].CustomerId);
                Assert.Equal(expected.ReportedUsers[0].Name, actual.ReportedUsers[0].Name);
                Assert.Equal(expected.ReportedUsers[0].Surname, actual.ReportedUsers[0].Surname);
                Assert.Equal(expected.ReportedUsers[0].Message, actual.ReportedUsers[0].Message);
            }
        }
    }
}