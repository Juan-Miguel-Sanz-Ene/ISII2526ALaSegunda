using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Ban
{
    internal class BanUser_PO : PageObject
    {

        // Inputs
        private readonly By inputReason = By.Id("banReason");
        private readonly By inputDescription = By.Id("banDescription");
        private readonly By inputStartDate = By.Id("banStartDate");
        private readonly By inputEndDate = By.Id("banEndDate");

        // User selection
        private readonly By userRows = By.CssSelector(".ban-user-row");

        // Buttons
        private readonly By btnBanUsers = By.Id("btnBanUsers");

        // Messages
        private readonly By successMessage = By.Id("banSuccessMessage");
        private readonly By errorMessage = By.Id("banErrorMessage");

        public BanUser_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/banusers");
            Pause();
        }

        public void FillBanForm(string reason, string description, string startDate, string endDate)
        {
            WaitForBeingVisible(inputReason);
            _driver.FindElement(inputReason).Clear();
            _driver.FindElement(inputReason).SendKeys(reason);

            WaitForBeingVisible(inputDescription);
            _driver.FindElement(inputDescription).Clear();
            _driver.FindElement(inputDescription).SendKeys(description);

            WaitForBeingVisible(inputStartDate);
            _driver.FindElement(inputStartDate).Clear();
            _driver.FindElement(inputStartDate).SendKeys(startDate);

            WaitForBeingVisible(inputEndDate);
            _driver.FindElement(inputEndDate).Clear();
            _driver.FindElement(inputEndDate).SendKeys(endDate);

            Pause();
        }

        public void SelectFirstUser()
        {
            WaitForBeingVisible(userRows);
            var firstCheckbox = _driver.FindElements(userRows)
                                       .First()
                                       .FindElement(By.CssSelector("input[type='checkbox']"));

            WaitForBeingClickable(By.CssSelector(".ban-user-row input[type='checkbox']"));
            firstCheckbox.Click();
            Pause();
        }

        public void Submit()
        {
            WaitForBeingVisible(btnBanUsers);
            WaitForBeingClickable(btnBanUsers);
            _driver.FindElement(btnBanUsers).Click();
            Pause();
        }

        public bool SuccessDisplayed()
        {
            return _driver.FindElements(successMessage).Any();
        }

        public bool ErrorDisplayed()
        {
            return _driver.FindElements(errorMessage).Any();
        }
    }
}
