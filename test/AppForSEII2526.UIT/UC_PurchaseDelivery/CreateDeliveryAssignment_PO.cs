using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class CreateDeliveryAssignment_PO : PageObject
    {
        // Form inputs
        private By inputDeliveryDriverId = By.Id("DelieveryDriver");
        private By inputDeliveryDeadline = By.Id("DeliveryDeadline");
        private By inputPersonalMessage = By.Id("PersonalMessage");
        private By inputExtraReward = By.Id("ExtraReward");

        // Buttons
        private By buttonSubmit = By.Id("Submit");
        private By buttonModifyPurchaseDeliveries = By.Id("ModifyPurchaseDeliveries");
        private By buttonDialogOK = By.Id("Button_DialogOK");
        private By buttonDialogCancel = By.Id("Button_DialogCancel");

        // Error message
        private By errorsShown = By.Id("ErrorsShown");

        // Table of purchase deliveries
        private By tableOfPurchaseDeliveries = By.Id("TableOfRentalItems");

        public CreateDeliveryAssignment_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillDeliveryAssignmentForm(string deliveryDriverId, string deliveryDeadline, 
            string personalMessage, string extraReward)
        {
            WaitForBeingVisible(inputDeliveryDriverId);
            WaitForBeingClickable(inputDeliveryDriverId);

            // Clear and fill Delivery Driver ID
            var driverIdElement = _driver.FindElement(inputDeliveryDriverId);
            driverIdElement.Clear();
            driverIdElement.SendKeys(deliveryDriverId);

            // Clear and fill Delivery Deadline
            var deadlineElement = _driver.FindElement(inputDeliveryDeadline);
            deadlineElement.Clear();
            deadlineElement.SendKeys(deliveryDeadline);

            // Clear and fill Personal Message
            var messageElement = _driver.FindElement(inputPersonalMessage);
            messageElement.Clear();
            messageElement.SendKeys(personalMessage);

            // Clear and fill Extra Reward
            var rewardElement = _driver.FindElement(inputExtraReward);
            rewardElement.Clear();
            rewardElement.SendKeys(extraReward);
        }

        public void SubmitForm()
        {
            WaitForBeingVisible(buttonSubmit);
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
        }

        public void ConfirmDialog()
        {
            WaitForBeingVisible(buttonDialogOK);
            WaitForBeingClickable(buttonDialogOK);
            _driver.FindElement(buttonDialogOK).Click();
        }

        public void CancelDialog()
        {
            WaitForBeingVisible(buttonDialogCancel);
            WaitForBeingClickable(buttonDialogCancel);
            _driver.FindElement(buttonDialogCancel).Click();
        }

        public void ClickModifyPurchaseDeliveries()
        {
            WaitForBeingVisible(buttonModifyPurchaseDeliveries);
            WaitForBeingClickable(buttonModifyPurchaseDeliveries);
            _driver.FindElement(buttonModifyPurchaseDeliveries).Click();
        }

        public bool CheckErrorMessageDisplayed(string expectedError)
        {
            WaitForBeingVisible(errorsShown);
            var errorText = _driver.FindElement(errorsShown).Text;
            return errorText.Contains(expectedError);
        }

        public bool CheckValidationErrorDisplayed(string expectedError)
        {
            WaitForBeingVisible(By.ClassName("validation-summary-errors"));
            var validationSummary = _driver.FindElement(By.ClassName("validation-summary-errors"));
            return validationSummary.Text.Contains(expectedError);
        }

        public bool IsOnDetailPage()
        {
            ImplicitWait(3);
            return _driver.Url.Contains("detaildeliveryassignment");
        }

        public bool CheckPurchaseDeliveriesInTable(List<string[]> expectedPurchaseDeliveries)
        {
            WaitForBeingVisible(tableOfPurchaseDeliveries);
            return CheckBodyTable(expectedPurchaseDeliveries, tableOfPurchaseDeliveries);
        }
    }
}