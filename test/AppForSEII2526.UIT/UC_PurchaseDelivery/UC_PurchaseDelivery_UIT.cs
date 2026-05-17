using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class UC_PurchaseDeliveries_UIT : UC_UIT
    {
        private SelectPurchaseOrdersForDelivery_PO selectPurchaseOrdersForDelivery_PO;

        private const int purchaseOrderId1 = 5;
        private const string purchaseOrderCity1 = "Ab";
        private const string purchaseOrderStreet1 = "Av españa";
        private const string purchaseOrderPostalCode1 = "02005";
        private const string purchaseOrderDate1 = "13/10/2025 0:00:00 +02:00";
        private const string purchaseOrderTotalPrice1 = "5";

        private const int purchaseOrderId2 = 1;
        private const string purchaseOrderCity2 = "Ab";
        private const string purchaseOrderStreet2 = "Av españa";
        private const string purchaseOrderPostalCode2 = "02003";
        private const string purchaseOrderDate2 = "13/10/2025 0:00:00 +02:00";
        private const string purchaseOrderTotalPrice2 = "10";

        public UC_PurchaseDeliveries_UIT(ITestOutputHelper output) : base(output)
        {
            selectPurchaseOrdersForDelivery_PO = new SelectPurchaseOrdersForDelivery_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("alejandro.gomez31@alu.uclm.es", "Alejandro@1234");
        }

        private void InitialStepsForPurchaseDelivery()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("CreateDeliveryAssignment"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateDeliveryAssignment")).Click();
        }

        [Fact(DisplayName = "UC1_BF_AF0 – SelectPurchaseOrdersForDelivery No Filtering")]
        [Trait("UseCase", "UC1_BF_AF0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_BF_AF0_SelectPurchaseOrdersForDelivery_No_Filtering_Test()
        {
            //Arrange
            InitialStepsForPurchaseDelivery();
            var expectedPurchaseOrders = new List<string[]> {
                new string[] { purchaseOrderId2.ToString(), purchaseOrderDate2, purchaseOrderTotalPrice2, purchaseOrderCity2, purchaseOrderStreet2, purchaseOrderPostalCode2 },
                new string[] { purchaseOrderId1.ToString(), purchaseOrderDate1, purchaseOrderTotalPrice1, purchaseOrderCity1, purchaseOrderStreet1, purchaseOrderPostalCode1 },
            };

            //Act
            selectPurchaseOrdersForDelivery_PO.SearchPurchaseOrders("", "");

            //Assert
            Assert.True(selectPurchaseOrdersForDelivery_PO.CheckListOfPurchaseOrders(expectedPurchaseOrders));
        }

        [Theory(DisplayName = "UC1_AF1 – SelectPurchaseOrdersForDelivery Filtering")]
        [InlineData(purchaseOrderId1, purchaseOrderDate1, purchaseOrderTotalPrice1, purchaseOrderCity1, purchaseOrderStreet1, purchaseOrderPostalCode1, "02005", "")]
        [InlineData(purchaseOrderId2, purchaseOrderDate2, purchaseOrderTotalPrice2, purchaseOrderCity2, purchaseOrderStreet2, purchaseOrderPostalCode2, "", "10")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF1_SelectPurchaseOrdersForDelivery_Filtering_Test(int purchaseOrderId, string purchaseOrderDate, string purchaseOrderTotalPrice, string purchaseOrderCity, string purchaseOrderStreet, string purchaseOrderPostalCode, string filterPostalCode, string filterPrice)
        {
            //Arrange
            InitialStepsForPurchaseDelivery();
            var expectedPurchaseOrders = new List<string[]> { new string[] { purchaseOrderId.ToString(), purchaseOrderDate, purchaseOrderTotalPrice, purchaseOrderCity, purchaseOrderStreet, purchaseOrderPostalCode }, };

            //Act
            selectPurchaseOrdersForDelivery_PO.SearchPurchaseOrders(filterPostalCode, filterPrice);

            //Assert

            Assert.True(selectPurchaseOrdersForDelivery_PO.CheckListOfPurchaseOrders(expectedPurchaseOrders));

        }

        [Fact(DisplayName = "UC1_AF2 – GetPurchaseOrdersToDelivery No Results")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF2_Purchase_Order_Not_Available()
        {
            //Arrange
            InitialStepsForPurchaseDelivery();
            //Act
            selectPurchaseOrdersForDelivery_PO.AddPurchaseOrderToDeliveryList(purchaseOrderId1.ToString());
            selectPurchaseOrdersForDelivery_PO.RemovePurchaseOrderToDeliveryList(purchaseOrderId1.ToString());

            //Assert

            Assert.True(selectPurchaseOrdersForDelivery_PO.DeliverNotAvailable());

        }


    }
}
