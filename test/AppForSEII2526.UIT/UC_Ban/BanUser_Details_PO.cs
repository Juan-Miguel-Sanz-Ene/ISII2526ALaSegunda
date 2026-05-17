using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Ban
{
    internal class BanUser_Details_PO : PageObject
    {

        // Campos de detalle
        private readonly By lblReason = By.Id("banDetailReason");
        private readonly By lblDescription = By.Id("banDetailDescription");
        private readonly By lblStartDate = By.Id("banDetailStartDate");
        private readonly By lblEndDate = By.Id("banDetailEndDate");

        // Lista de usuarios baneados
        private readonly By userRows = By.CssSelector(".ban-detail-user-row");

        // Botón volver
        private readonly By btnBack = By.Id("btnBackToList");

        public BanUser_Details_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void NavigateTo(int banId)
        {
            _driver.Navigate().GoToUrl($"https://localhost:5001/banreport/{banId}");
            Pause();
        }

        public string GetReason()
        {
            WaitForBeingVisible(lblReason);
            return _driver.FindElement(lblReason).Text;
        }

        public string GetDescription()
        {
            WaitForBeingVisible(lblDescription);
            return _driver.FindElement(lblDescription).Text;
        }

        public string GetStartDate()
        {
            WaitForBeingVisible(lblStartDate);
            return _driver.FindElement(lblStartDate).Text;
        }

        public string GetEndDate()
        {
            WaitForBeingVisible(lblEndDate);
            return _driver.FindElement(lblEndDate).Text;
        }

        public int CountBannedUsers()
        {
            WaitForBeingVisible(userRows);
            return _driver.FindElements(userRows).Count;
        }

        public void ClickBack()
        {
            WaitForBeingVisible(btnBack);
            WaitForBeingClickable(btnBack);
            _driver.FindElement(btnBack).Click();
            Pause();
        }
    }
}
