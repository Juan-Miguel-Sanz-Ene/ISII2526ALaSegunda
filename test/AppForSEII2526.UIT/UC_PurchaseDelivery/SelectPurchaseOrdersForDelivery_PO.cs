using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class SelectPurchaseOrdersForDelivery_PO : PageObject
    {
        By inputPostalcode = By.Id("inputPostalcode");
        By inputTotalPrice = By.Id("inputTotalPrice");
        By buttonSearchPurchaseOrders = By.Id("searchPurchaseOrders");
        By tableOfPurchaseOrdersBy = By.Id("TableOfPurchaseOrders");
        By assignDeliveryButton = By.Id("assignDeliveryButton");

        public SelectPurchaseOrdersForDelivery_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchPurchaseOrders(string postalcode, string totalPrice)
        {
            //wait for the webelement to be clickable
            WaitForBeingVisible(inputPostalcode);
            WaitForBeingClickable(inputPostalcode);
            WaitForBeingVisible(inputTotalPrice);
            WaitForBeingClickable(inputTotalPrice);
            WaitForBeingVisible(buttonSearchPurchaseOrders);
            _driver.FindElement(inputPostalcode).SendKeys(postalcode);
            _driver.FindElement(inputTotalPrice).SendKeys(totalPrice);
            _driver.FindElement(buttonSearchPurchaseOrders).Click();


        }

        public void AddPurchaseOrderToDeliveryList(string purchaseOrderId)
        {
            WaitForBeingVisible(By.Id("movieToRent_" + purchaseOrderId));
            WaitForBeingClickable(By.Id("movieToRent_" + purchaseOrderId));
            _driver.FindElement(By.Id("movieToRent_" + purchaseOrderId)).Click();
        }

        public void RemovePurchaseOrderToDeliveryList(string purchaseOrderId)
        {
            WaitForBeingVisible(By.Id("purchaseDeliveryToDelete_" + purchaseOrderId));
            WaitForBeingClickable(By.Id("purchaseDeliveryToDelete_" + purchaseOrderId));
            _driver.FindElement(By.Id("purchaseDeliveryToDelete_" + purchaseOrderId)).Click();
        }

        public bool DeliverNotAvailable()
        {
            return _driver.FindElement(assignDeliveryButton).Displayed == false;
        }

        public bool CheckListOfPurchaseOrders(List<string[]> expectedPurchaseOrders)
        {
            WaitForBeingVisible(tableOfPurchaseOrdersBy);
            return CheckBodyTable(expectedPurchaseOrders, tableOfPurchaseOrdersBy);
        }
    }
}
