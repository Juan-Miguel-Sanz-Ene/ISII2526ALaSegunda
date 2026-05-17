using AppForMovies.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UCPurchase_UIT : UC_UIT
    {
        private SelectProductsForPurchasing_PO selectProducts_PO;
        private CreatePurchase_PO createPurchase_PO;
        private PurchaseDetail_PO purchaseDetail_PO;

        public UCPurchase_UIT(ITestOutputHelper output) : base(output)
        {
            selectProducts_PO = new SelectProductsForPurchasing_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
            purchaseDetail_PO = new PurchaseDetail_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("pepe@uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchase()
        {
            Precondition_perform_login();

            try
            {
                System.Threading.Thread.Sleep(1000);
                selectProducts_PO.OpenFromMenu();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                selectProducts_PO.OpenFromMenu();
            }
        }

        private void AddProductAndProceed(string productName)
        {
            selectProducts_PO.SearchProducts(productName, "");
            selectProducts_PO.AddProductToCart(productName);
            selectProducts_PO.ClickPurchaseProducts();
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_BF_SuccessfulPurchase()
        {
            InitialStepsForPurchase();

            string productName = "Jacket";
            AddProductAndProceed(productName);

            createPurchase_PO.FillPurchaseForm("Pepe", "Perez", "Calle Real, 1", "Albacete", "02001");
            createPurchase_PO.SelectFirstAvailablePaymentMethod();
            createPurchase_PO.Submit();

            Assert.True(purchaseDetail_PO.CheckCustomerAndAddress("Pepe Perez", "Calle Real, 1", "Albacete", "02001"));
            Assert.True(purchaseDetail_PO.HasPurchasedProduct(productName));
            Assert.True(purchaseDetail_PO.PaymentMethodIsNotEmpty());
            Assert.True(purchaseDetail_PO.IsStateDisplayed("Request"));
            Assert.True(purchaseDetail_PO.IsTotalPriceCorrect("20"));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF0_NoProductsToPurchase()
        {
            InitialStepsForPurchase();
            selectProducts_PO.SearchProducts("NonExistentProductXYZ", "");
            Assert.True(selectProducts_PO.IsNoProductsResultShown());
        }

        [Theory]
        [InlineData("Jacket", "")]
        [InlineData("", "Red")]
        [InlineData("Jacket", "Red")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF1_FilteringProducts(string name, string colour)
        {
            InitialStepsForPurchase();
            selectProducts_PO.SearchProducts(name, colour);
            Assert.False(selectProducts_PO.IsNoProductsResultShown());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF1_Filtering_NoResults()
        {
            InitialStepsForPurchase();
            selectProducts_PO.SearchProducts("NonExistent", "Pink");
            Assert.True(selectProducts_PO.IsNoProductsResultShown());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF3_EmptyCart_PurchaseNotAvailable()
        {
            InitialStepsForPurchase();
            Assert.True(selectProducts_PO.PurchaseNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF4_ModifyCart_ChangeQuantity()
        {
            InitialStepsForPurchase();

            string productName = "Jacket";
            selectProducts_PO.SearchProducts(productName, "");
            selectProducts_PO.AddProductToCart(productName);

            selectProducts_PO.IncreaseProductQuantity(productName);

            int currentQty = selectProducts_PO.GetProductQuantityInCart(productName);
            Assert.True(currentQty >= 2, $"Expected quantity >= 2, but was {currentQty}");

            selectProducts_PO.DecreaseProductQuantity(productName);

            currentQty = selectProducts_PO.GetProductQuantityInCart(productName);
            Assert.True(currentQty == 1, $"Expected quantity to be 1, but was {currentQty}");
        }

        [Theory]
        [InlineData("", "Perez", "Street 1", "City", "00000", "The NameCustomer field is required.")]
        [InlineData("Pepe", "", "Street 1", "City", "00000", "The SurnameCustomer field is required.")]
        [InlineData("Pepe", "Perez", "", "City", "00000", "The Street field is required.")]
        [InlineData("Pepe", "Perez", "Street 1", "", "00000", "The City field is required.")]
        [InlineData("Pepe", "Perez", "Street 1", "City", "", "The PostalCode field is required.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF5_ValidationErrors(string name, string surname, string street, string city, string zip, string expectedError)
        {
            InitialStepsForPurchase();
            AddProductAndProceed("Jacket");

            createPurchase_PO.FillPurchaseForm(name, surname, street, city, zip);
            createPurchase_PO.SelectFirstAvailablePaymentMethod();
            createPurchase_PO.Submit();

            Assert.True(createPurchase_PO.CheckValidationErrorDisplayed(expectedError));
           
        }
    }
}