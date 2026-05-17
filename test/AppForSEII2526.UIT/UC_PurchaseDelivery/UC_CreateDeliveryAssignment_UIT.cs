using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class UC_CreateDeliveryAssignment_UIT : UC_UIT
    {
        private SelectPurchaseOrdersForDelivery_PO selectPurchaseOrdersForDelivery_PO;
        private CreateDeliveryAssignment_PO createDeliveryAssignment_PO;
        private GetDetailsDeliveryAssignment_PO getDetailsDeliveryAssignment_PO;

        private const int purchaseOrderId1 = 5;

        public UC_CreateDeliveryAssignment_UIT(ITestOutputHelper output) : base(output)
        {
            selectPurchaseOrdersForDelivery_PO = new SelectPurchaseOrdersForDelivery_PO(_driver, _output);
            createDeliveryAssignment_PO = new CreateDeliveryAssignment_PO(_driver, _output);
            getDetailsDeliveryAssignment_PO = new GetDetailsDeliveryAssignment_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("alejandro.gomez31@alu.uclm.es", "Alejandro@1234");
        }

        private void NavigateToSelectPurchaseOrdersAndAddOrder(int purchaseOrderId)
        {
            Precondition_perform_login();
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("CreateDeliveryAssignment"));
            _driver.FindElement(By.Id("CreateDeliveryAssignment")).Click();
            selectPurchaseOrdersForDelivery_PO.SearchPurchaseOrders("", "");
            selectPurchaseOrdersForDelivery_PO.AddPurchaseOrderToDeliveryList(purchaseOrderId.ToString());
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("assignDeliveryButton"));
            _driver.FindElement(By.Id("assignDeliveryButton")).Click();
        }

        [Fact(DisplayName = "UC2_BF – CreateDeliveryAssignment Success")]
        [Trait("UseCase", "UC2_BF")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_BF_CreateDeliveryAssignment_Success_Test()
        {
            // Arrange
            NavigateToSelectPurchaseOrdersAndAddOrder(purchaseOrderId1);

            string deliveryDriverId = "1";
            string deliveryDeadline = "31/12/2025";
            string personalMessage = "Please, deliver fast";
            string extraReward = "10";

            // Act
            createDeliveryAssignment_PO.FillDeliveryAssignmentForm(
                deliveryDriverId,
                deliveryDeadline,
                personalMessage,
                extraReward);
            createDeliveryAssignment_PO.SubmitForm();
            createDeliveryAssignment_PO.ConfirmDialog();

            // Assert
            Assert.True(createDeliveryAssignment_PO.IsOnDetailPage());
        }

        [Fact(DisplayName = "UC2_AF0 – CreateDeliveryAssignment DeliveryDriver Not Found")]
        [Trait("UseCase", "UC2_AF0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF0_CreateDeliveryAssignment_DeliveryDriverNotFound_Test()
        {
            // Arrange
            NavigateToSelectPurchaseOrdersAndAddOrder(purchaseOrderId1);

            string deliveryDriverId = "999"; // Non-existent driver
            string deliveryDeadline = "31/12/2025";
            string personalMessage = "Please, deliver fast";
            string extraReward = "10";

            // Act
            createDeliveryAssignment_PO.FillDeliveryAssignmentForm(
                deliveryDriverId, 
                deliveryDeadline, 
                personalMessage, 
                extraReward);
            createDeliveryAssignment_PO.SubmitForm();
            createDeliveryAssignment_PO.ConfirmDialog();

            // Assert
            Assert.True(createDeliveryAssignment_PO.CheckErrorMessageDisplayed("Delivery driver does not exist"));
        }

        [Fact(DisplayName = "UC2_AF2 – CreateDeliveryAssignment Invalid PersonalMessage")]
        [Trait("UseCase", "UC2_AF2")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF2_CreateDeliveryAssignment_InvalidPersonalMessage_Test()
        {
            // Arrange
            NavigateToSelectPurchaseOrdersAndAddOrder(purchaseOrderId1);

            string deliveryDriverId = "1";
            string deliveryDeadline = "31/12/2025";
            string personalMessage = "Fast delivery"; // Does not start with "Please,"
            string extraReward = "10";

            // Act
            createDeliveryAssignment_PO.FillDeliveryAssignmentForm(
                deliveryDriverId, 
                deliveryDeadline, 
                personalMessage, 
                extraReward);
            createDeliveryAssignment_PO.SubmitForm();
            createDeliveryAssignment_PO.ConfirmDialog();

            // Assert - Should show error message about PersonalMessage
            Assert.True(createDeliveryAssignment_PO.CheckErrorMessageDisplayed("Please,"));
        }

        [Fact(DisplayName = "UC2_AF3 – CreateDeliveryAssignment Invalid DeliveryDeadline")]
        [Trait("UseCase", "UC2_AF3")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF3_CreateDeliveryAssignment_InvalidDeliveryDeadline_Test()
        {
            // Arrange
            NavigateToSelectPurchaseOrdersAndAddOrder(purchaseOrderId1);

            string deliveryDriverId = "1";
            string deliveryDeadline = "01/01/2020";
            string personalMessage = "Please, deliver fast";
            string extraReward = "10";

            // Act
            createDeliveryAssignment_PO.FillDeliveryAssignmentForm(
                deliveryDriverId,
                deliveryDeadline,
                personalMessage,
                extraReward);
            createDeliveryAssignment_PO.SubmitForm();
            createDeliveryAssignment_PO.ConfirmDialog();

            // Assert
            Assert.True(createDeliveryAssignment_PO.CheckErrorMessageDisplayed("Delivery deadline must be later than now"));
        }

        [Fact(DisplayName = "UC2_AF2+AF3 - Test Case For Exam")]
        [Trait("UseCase", "UC2_AF2+AF3")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF2_AF3_Exam_Test()
        {
            // Arrange
            Precondition_perform_login();
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("CreateDeliveryAssignment"));
            _driver.FindElement(By.Id("CreateDeliveryAssignment")).Click();

            string deliveryDriverId = "1";
            string deliveryDriverName = "Juan";
            string deliveryDeadline = "31/12/2025";
            string personalMessage = "Please, deliver fast";
            string extraReward = "10";

            var expectedPurchaseDeliveries = new List<string[]>
                {
                    new string[] { "13/10/2025", "Av españa", "Ab", "02005", "5 €" }
                };

            // Act
            selectPurchaseOrdersForDelivery_PO.AddPurchaseOrderToDeliveryList("1");
            selectPurchaseOrdersForDelivery_PO.SearchPurchaseOrders("02005", ""); 
            selectPurchaseOrdersForDelivery_PO.AddPurchaseOrderToDeliveryList("5");
            selectPurchaseOrdersForDelivery_PO.RemovePurchaseOrderToDeliveryList("1");

            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("assignDeliveryButton"));
            _driver.FindElement(By.Id("assignDeliveryButton")).Click();

            createDeliveryAssignment_PO.FillDeliveryAssignmentForm(
                deliveryDriverId,
                deliveryDeadline,
                personalMessage,
                extraReward);
            createDeliveryAssignment_PO.SubmitForm();
            createDeliveryAssignment_PO.ConfirmDialog();

            // Assert
            Assert.True(getDetailsDeliveryAssignment_PO.CheckListOfPurchaseDeliveries(expectedPurchaseDeliveries));
            Assert.True(getDetailsDeliveryAssignment_PO.CheckDeliveryAssignmentDetailWithoutID(deliveryDriverName, personalMessage, deliveryDeadline, extraReward));
            Assert.True(createDeliveryAssignment_PO.IsOnDetailPage());
        }


    }
}