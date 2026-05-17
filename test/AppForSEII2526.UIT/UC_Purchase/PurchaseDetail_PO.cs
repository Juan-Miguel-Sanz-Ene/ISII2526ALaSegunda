namespace AppForSEII2526.UIT.UC_Purchase
{
    public class PurchaseDetail_PO : PageObject
    {
        By nameSurname = By.Id("NameSurname");
        By deliveryAddress = By.Id("DeliveryAddress");
        By paymentMethod = By.Id("PaymentMethod");
        By purchaseDate = By.Id("PurchaseDate");
        By state = By.Id("State");
        By totalPrice = By.Id("TotalPrice");
        By purchasedProductsTable = By.Id("PurchasedProducts");

        public PurchaseDetail_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckCustomerAndAddress(string expectedNameSurname, string expectedStreet, string expectedCity, string expectedPostalCode)
        {
            Pause();
            WaitForBeingVisible(nameSurname);

            var actualNameSurname = _driver.FindElement(nameSurname).Text;
            var actualDeliveryAddress = _driver.FindElement(deliveryAddress).Text;

            bool result = true;
            result = result && actualNameSurname.Contains(expectedNameSurname);
            result = result && actualDeliveryAddress.Contains(expectedStreet);
            result = result && actualDeliveryAddress.Contains(expectedCity);
            result = result && actualDeliveryAddress.Contains(expectedPostalCode);
            return result;
        }

        public bool HasPurchasedProduct(string productName)
        {
            WaitForBeingVisible(purchasedProductsTable);
            WaitForBeingVisible(By.Id($"PurchaseItem_{productName}"));
            return _driver.FindElement(By.Id($"PurchaseItem_{productName}")).Displayed;
        }

        public bool PaymentMethodIsNotEmpty()
        {
            WaitForBeingVisible(paymentMethod);
            var value = _driver.FindElement(paymentMethod).Text;
            return !string.IsNullOrWhiteSpace(value);
        }

        public bool IsStateDisplayed(string expectedState)
        {
            WaitForBeingVisible(state);
            var actualState = _driver.FindElement(state).Text;
            return actualState.Contains(expectedState);
        }

        public bool IsTotalPriceCorrect(string expectedPrice)
        {
            WaitForBeingVisible(totalPrice);
            var actualPrice = _driver.FindElement(totalPrice).Text;
            return actualPrice.Contains(expectedPrice);
        }
    }
}