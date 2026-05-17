using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace AppForSEII2526.UT.BanUser_test
{
    public class ComplaintsController_test : AppForMovies4SqliteUT
    {
        public ComplaintsController_test()
        {
            var user1 = new ApplicationUser
            {
                Id = "1",
                Name = "Juan",
                Surname = "García",
                AccountCreationDate = new DateTime(2025, 12, 12),
                UserName = "juan",
                Email = "juan@test.es",
                Address = "Calle Falsa 123"
            };

            var user2 = new ApplicationUser
            {
                Id = "2",
                Name = "Jaime",
                Surname = "Sanchez",
                AccountCreationDate = new DateTime(2025, 11, 4),
                UserName = "jaime",
                Email = "jaime@test.es",
                Address = "Calle Falsa 124"
            };

            var type1 = new ComplaintType { ID = 1, Name = "Producto" };
            var type2 = new ComplaintType { ID = 2, Name = "Servicio" };

            var complaints = new List<Complaint>
            {
                new Complaint { ID = 1, ComplaintDate = DateTime.UtcNow, Description = "Problema producto", Processed = false, User = user1, Type = type1 },
                new Complaint { ID = 2, ComplaintDate = DateTime.UtcNow, Description = "Problema servicio", Processed = false, User = user2, Type = type2 },
                new Complaint { ID = 3, ComplaintDate = DateTime.UtcNow, Description = "Ya procesada", Processed = true, User = user1, Type = type1 }
            };

            _context.Users.AddRange(user1, user2);
            _context.ComplaintTypes.AddRange(type1, type2);
            _context.Complaints.AddRange(complaints);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetPendingComplaints()
        {
            var expectedAll = new List<UserComplaintsDTO>
            {
                new UserComplaintsDTO { Name = "Juan", Surname = "García", AccountCreationDate = new DateTime(2025, 12, 12), ComplaintCount = 1, ComplaintTypes = new List<string>{ "Producto" } },
                new UserComplaintsDTO { Name = "Jaime", Surname = "Sanchez", AccountCreationDate = new DateTime(2025, 11, 4), ComplaintCount = 1, ComplaintTypes = new List<string>{ "Servicio" } }
            };

            var expectedBySurname = new List<UserComplaintsDTO>
            {
                new UserComplaintsDTO { Name = "Juan", Surname = "García", AccountCreationDate = new DateTime(2025, 12, 12), ComplaintCount = 1, ComplaintTypes = new List<string>{ "Producto" } }
            };

            var expectedByType = new List<UserComplaintsDTO>
            {
                new UserComplaintsDTO { Name = "Jaime", Surname = "Sanchez", AccountCreationDate = new DateTime(2025, 11, 4), ComplaintCount = 1, ComplaintTypes = new List<string>{ "Servicio" } }
            };

            var expectedNone = new List<UserComplaintsDTO>();

            return new List<object[]>
            {
                new object[] { null, null, expectedAll },
                new object[] { "García", null, expectedBySurname },
                new object[] { null, "Servicio", expectedByType },
                new object[] { "NoExiste", null, expectedNone }
            };
        }

        [Theory(DisplayName = "UC_BF_AF1_AF2 – GetPendingComplaints")]
        [Trait("UseCase", "UC_BF_AF1_AF2")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_GetPendingComplaints))]
        public async Task UC_BF_AF1_AF2_GetPendingComplaints_Test(
            string? surname,
            string? complaintType,
            IList<UserComplaintsDTO> expected)
        {
            
            var mockLogger = new Mock<ILogger<ComplaintsController>>();
            var controller = new ComplaintsController(_context, mockLogger.Object);

           
            var result = await controller.GetPendingComplaints(
                new ComplaintFilterDTO { Surname = surname, ComplaintType = complaintType });

            
            var actionResult = Assert.IsType<ActionResult<ComplaintsResponseDTO>>(result);
            var dto = Assert.IsType<ComplaintsResponseDTO>(actionResult.Value);

            if (!expected.Any())
            {

                Assert.False(dto.HasComplaints);
                Assert.Equal("No users with complaints to be addressed.", dto.Message);

                
                mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No users with complaints")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }
            else
            {
                
                Assert.True(dto.HasComplaints);

                var expectedOrdered = expected.OrderBy(u => u.Surname).ThenBy(u => u.Name).ToList();
                var actualOrdered = dto.Users.OrderBy(u => u.Surname).ThenBy(u => u.Name).ToList();

                Assert.Equal(expectedOrdered.Count, actualOrdered.Count);

                for (int i = 0; i < expectedOrdered.Count; i++)
                {
                    Assert.Equal(expectedOrdered[i].Name, actualOrdered[i].Name);
                    Assert.Equal(expectedOrdered[i].Surname, actualOrdered[i].Surname);
                    Assert.Equal(expectedOrdered[i].AccountCreationDate, actualOrdered[i].AccountCreationDate);
                    Assert.Equal(expectedOrdered[i].ComplaintCount, actualOrdered[i].ComplaintCount);
                    Assert.Equal(expectedOrdered[i].ComplaintTypes, actualOrdered[i].ComplaintTypes);
                }
            }
        }
    }
}
