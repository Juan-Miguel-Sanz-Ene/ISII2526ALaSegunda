using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectProductsForPurchasing_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputColour = By.Id("inputColour");
        By btnSearch = By.Id("btnSearch");
        By productsTable = By.Id("productsTable");
        By btnPurchase = By.Id("btnPurchase");
        By shoppingCart = By.Id("shoppingCart");
        By menuSelectProducts = By.Id("menuSelectProducts");

        public SelectProductsForPurchasing_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void OpenFromMenu()
        {
            WaitForBeingVisible(menuSelectProducts);
            WaitForBeingClickable(menuSelectProducts);

            try
            {
                _driver.FindElement(menuSelectProducts).Click();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                _driver.FindElement(menuSelectProducts).Click();
            }
        }

        public void SearchProducts(string name, string colour)
        {
            WaitForBeingVisible(inputName);
            WaitForBeingClickable(inputName);
            WaitForBeingVisible(inputColour);

            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);

            _driver.FindElement(inputColour).Clear();
            _driver.FindElement(inputColour).SendKeys(colour);

            WaitForBeingClickable(btnSearch);
            _driver.FindElement(btnSearch).Click();
            Pause();
        }

        public void AddProductToCart(string productName)
        {
            By btnAdd = By.Id($"btnAdd_{productName}");
            WaitForBeingVisible(btnAdd);
            WaitForBeingClickable(btnAdd);

            try
            {
                _driver.FindElement(btnAdd).Click();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                //Fix for when the table refresh too fast
                _driver.FindElement(btnAdd).Click();
            }

            WaitForBeingVisible(shoppingCart);
            Pause();
        }

        public void ClickPurchaseProducts()
        {
            WaitForBeingVisible(btnPurchase);
            WaitForBeingClickable(btnPurchase);
            _driver.FindElement(btnPurchase).Click();
            Pause();
        }
        public void RemoveProductFromCart(string productName)
        {
            By btnRemove = By.Id($"btnRemove_{productName}");
            WaitForBeingVisible(btnRemove);
            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }
        public void IncreaseProductQuantity(string productName)
        {
            By btnIncrease = By.Id($"btnIncrease_{productName}");
            WaitForBeingVisible(btnIncrease);
            WaitForBeingClickable(btnIncrease);
            _driver.FindElement(btnIncrease).Click();
        }

        public void DecreaseProductQuantity(string productName)
        {
            By btnDecrease = By.Id($"btnDecrease_{productName}");
            WaitForBeingVisible(btnDecrease);
            WaitForBeingClickable(btnDecrease);
            _driver.FindElement(btnDecrease).Click();
        }

        public int GetProductQuantityInCart(string productName)
        {
           
            System.Threading.Thread.Sleep(500);

            By qtySpan = By.Id($"qty_{productName}");
            WaitForBeingVisible(qtySpan);
            var qtyText = _driver.FindElement(qtySpan).Text;

            return int.TryParse(qtyText, out int qty) ? qty : 0;
        }

        public bool CheckProductsList(List<string[]> expectedProducts)
        {
            WaitForBeingVisible(productsTable);
            return CheckBodyTable(expectedProducts, productsTable);
        }

        public bool IsNoProductsResultShown()
        {
            System.Threading.Thread.Sleep(500);
            WaitForBeingVisible(productsTable);
            var table = _driver.FindElement(productsTable);
            var bodyText = table.FindElement(By.TagName("tbody")).Text;
            return bodyText.Contains("No products");
        }

        public bool PurchaseNotAvailable()
        {
            return _driver.FindElements(btnPurchase).Count == 0;
        }

        public bool IsProductsTableVisible()
        {
            WaitForBeingVisible(productsTable);
            return _driver.FindElement(productsTable).Displayed;
        }
    }
}