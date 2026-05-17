using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Ban
{
    internal class SelectUsersForBan_PO : PageObject
    {
        // Inputs
        By inputSurname = By.Id("inputSurname");
        By inputComplaintType = By.Id("inputComplaintType");

        // Buttons
        By btnSearch = By.Id("btnSearchUsers");
        By btnBanSelected = By.Id("btnBanSelected");

        // Table
        By usersTable = By.Id("usersTable");

        // Menu
        By menuSelectUsers = By.Id("menuSelectUsers");

        // Cart-like area (selected users)
        By selectedUsersPanel = By.Id("selectedUsersPanel");

        public SelectUsersForBan_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void OpenFromMenu()
        {
            WaitForBeingVisible(menuSelectUsers);
            WaitForBeingClickable(menuSelectUsers);

            try
            {
                _driver.FindElement(menuSelectUsers).Click();
            }
            catch (StaleElementReferenceException)
            {
                _driver.FindElement(menuSelectUsers).Click();
            }
        }

        public void SearchUsers(string surname, string complaintType)
        {
            WaitForBeingVisible(inputSurname);
            WaitForBeingClickable(inputSurname);
            WaitForBeingVisible(inputComplaintType);

            _driver.FindElement(inputSurname).Clear();
            _driver.FindElement(inputSurname).SendKeys(surname);

            _driver.FindElement(inputComplaintType).Clear();
            _driver.FindElement(inputComplaintType).SendKeys(complaintType);

            WaitForBeingClickable(btnSearch);
            _driver.FindElement(btnSearch).Click();
            Pause();
        }

        public void AddUserToSelection(string userId)
        {
            By btnAdd = By.Id($"btnAdd_{userId}");
            WaitForBeingVisible(btnAdd);
            WaitForBeingClickable(btnAdd);

            try
            {
                _driver.FindElement(btnAdd).Click();
            }
            catch (StaleElementReferenceException)
            {
                _driver.FindElement(btnAdd).Click();
            }

            WaitForBeingVisible(selectedUsersPanel);
            Pause();
        }

        public void RemoveUserFromSelection(string userId)
        {
            By btnRemove = By.Id($"btnRemove_{userId}");
            WaitForBeingVisible(btnRemove);
            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }

        public bool IsUsersTableVisible()
        {
            WaitForBeingVisible(usersTable);
            return _driver.FindElement(usersTable).Displayed;
        }

        public bool IsNoUsersResultShown()
        {
            System.Threading.Thread.Sleep(500);
            WaitForBeingVisible(usersTable);
            var table = _driver.FindElement(usersTable);
            var bodyText = table.FindElement(By.TagName("tbody")).Text;
            return bodyText.Contains("No users");
        }

        public bool BanNotAvailable()
        {
            return _driver.FindElements(btnBanSelected).Count == 0;
        }

        public void ClickBanSelected()
        {
            WaitForBeingVisible(btnBanSelected);
            WaitForBeingClickable(btnBanSelected);
            _driver.FindElement(btnBanSelected).Click();
            Pause();
        }

        public bool CheckUsersList(List<string[]> expectedUsers)
        {
            WaitForBeingVisible(usersTable);
            return CheckBodyTable(expectedUsers, usersTable);
        }
    }
}
