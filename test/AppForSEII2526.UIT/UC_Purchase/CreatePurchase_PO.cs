using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_PO : PageObject
    {
        By inputCustomerName = By.Id("NameCustomer");
        By inputCustomerSurname = By.Id("SurnameCustomer");
        By inputStreet = By.Id("Street");
        By inputCity = By.Id("City");
        By inputPostalCode = By.Id("PostalCode");
        By selectPaymentMethod = By.Id("PaymentMethodId");
        By btnConfirmPurchase = By.Id("btnConfirmPurchase");
        By btnModifyPurchase = By.Id("modifyPurchaseItems");

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPurchaseForm(string name, string surname, string street, string city, string zip)
        {
            WaitForBeingVisible(inputCustomerName);
            WaitForBeingVisible(inputCustomerSurname);
            WaitForBeingVisible(inputStreet);
            WaitForBeingVisible(inputCity);
            WaitForBeingVisible(inputPostalCode);

            _driver.FindElement(inputCustomerName).Clear();
            _driver.FindElement(inputCustomerName).SendKeys(name);

            _driver.FindElement(inputCustomerSurname).Clear();
            _driver.FindElement(inputCustomerSurname).SendKeys(surname);

            _driver.FindElement(inputStreet).Clear();
            _driver.FindElement(inputStreet).SendKeys(street);

            _driver.FindElement(inputCity).Clear();
            _driver.FindElement(inputCity).SendKeys(city);

            _driver.FindElement(inputPostalCode).Clear();
            _driver.FindElement(inputPostalCode).SendKeys(zip);
            Pause();
        }

        public void SelectFirstAvailablePaymentMethod()
        {
            WaitForBeingVisible(selectPaymentMethod);
            var select = new SelectElement(_driver.FindElement(selectPaymentMethod));
            select.SelectByIndex(1);
            Pause();
        }

        public void Submit()
        {
            WaitForBeingVisible(btnConfirmPurchase);
            WaitForBeingClickable(btnConfirmPurchase);
            _driver.FindElement(btnConfirmPurchase).Click();
            Pause();
        }

        public bool CheckValidationErrorDisplayed(string expectedError)
        {
            WaitForBeingVisible(By.ClassName("validation-message"));
            var errorElements = _driver.FindElements(By.ClassName("validation-message"));
            return errorElements.Any(error => error.Text.Contains(expectedError));
        }
        public void ClickModifyPurchase()
        {
            WaitForBeingVisible(btnModifyPurchase);
            WaitForBeingClickable(btnModifyPurchase);
            _driver.FindElement(btnModifyPurchase).Click();
        }
    }
}