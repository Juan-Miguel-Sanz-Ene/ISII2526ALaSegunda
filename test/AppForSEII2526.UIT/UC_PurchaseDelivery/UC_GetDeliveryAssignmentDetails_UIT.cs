using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class UC_GetDeliveryAssignmentDetails_UIT : UC_UIT
    {
        private GetDetailsDeliveryAssignment_PO getDetailsDeliveryAssignment_PO;

        public UC_GetDeliveryAssignmentDetails_UIT(ITestOutputHelper output) : base(output)
        {
            getDetailsDeliveryAssignment_PO = new GetDetailsDeliveryAssignment_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("alejandro.gomez31@alu.uclm.es", "Alejandro@1234");
        }

        private void NavigateToDeliveryAssignmentDetail(int deliveryAssignmentId)
        {
            Precondition_perform_login();
            _driver.Navigate().GoToUrl(_URI + $"purchasedelivery/detaildeliveryassignment?DeliveryAssignmentID={deliveryAssignmentId}");
        }

        [Fact(DisplayName = "UC2_BF_AF0 – GetDetailsDeliveryAssignment Success")]
        [Trait("UseCase", "UC2_BF_AF0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_BF_AF0_GetDetailsDeliveryAssignment_Success_Test()
        {
            // Arrange
            int deliveryAssignmentId = 7;
            string expectedId = "7";
            string expectedDeliveryDriverName = "Juan";
            string expectedPersonalMessage = "Please,";
            string expectedDeliveryAssignmentDone = "11/12/2025 12:00:00";
            string expectedExtraReward = "0";

            var expectedPurchaseDeliveries = new List<string[]>
                {
                    new string[] { "13/10/2025", "Av españa", "Ab", "02005", "5 €" }
                };

            // Act
            NavigateToDeliveryAssignmentDetail(deliveryAssignmentId);

            // Assert
            Assert.True(getDetailsDeliveryAssignment_PO.CheckDeliveryAssignmentDetail(
                expectedId,
                expectedDeliveryDriverName,
                expectedPersonalMessage,
                expectedDeliveryAssignmentDone,
                expectedExtraReward));

            Assert.True(getDetailsDeliveryAssignment_PO.CheckListOfPurchaseDeliveries(expectedPurchaseDeliveries));
        }

        [Fact(DisplayName = "UC2_AF1 – GetDetailsDeliveryAssignment Not Found")]
        [Trait("UseCase", "UC2_AF1")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_GetDetailsDeliveryAssignment_NotFound_Test()
        {
            // Arrange
            int deliveryAssignmentId = 77;
            string expectedErrorMessage = "Error 204:";

            // Act
            NavigateToDeliveryAssignmentDetail(deliveryAssignmentId);

            // Assert
            Assert.True(getDetailsDeliveryAssignment_PO.CheckErrorMessage(expectedErrorMessage));
        }
    }
}