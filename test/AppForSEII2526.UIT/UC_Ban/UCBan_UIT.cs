using AppForMovies.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Ban
{
    public class UCBan_UIT : UC_UIT
    {
        private SelectUsersForBan_PO selectUsers_PO;
        private BanUser_PO banUser_PO;
        private BanUser_Details_PO banDetails_PO;

        public UCBan_UIT(ITestOutputHelper output) : base(output)
        {
            selectUsers_PO = new SelectUsersForBan_PO(_driver, _output);
            banUser_PO = new BanUser_PO(_driver, _output);
            banDetails_PO = new BanUser_Details_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("admin@uclm.es", "Password1234%");
        }

        private void InitialStepsForBan()
        {
            Precondition_perform_login();

            try
            {
                System.Threading.Thread.Sleep(1000);
                selectUsers_PO.OpenFromMenu();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                selectUsers_PO.OpenFromMenu();
            }
        }

        private void AddUserAndProceed(string userId)
        {
            selectUsers_PO.SearchUsers(userId, "");
            selectUsers_PO.AddUserToSelection(userId);
            selectUsers_PO.ClickBanSelected();
        }

        // ---------------------------------------------------------
        // UC_BF — BAN SUCCESSFUL
        // ---------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_BF_SuccessfulBan()
        {
            InitialStepsForBan();

            string userId = "123"; // Ajusta según tus datos
            AddUserAndProceed(userId);

            banUser_PO.FillBanForm("Abuse", "User reported multiple times", "2026-05-20", "2026-06-20");
            banUser_PO.Submit();

            Assert.True(banUser_PO.SuccessDisplayed());
        }

        // ---------------------------------------------------------
        // UC_AF0 — NO USERS FOUND
        // ---------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF0_NoUsersFound()
        {
            InitialStepsForBan();
            selectUsers_PO.SearchUsers("NonExistentUserXYZ", "");
            Assert.True(selectUsers_PO.IsNoUsersResultShown());
        }

        // ---------------------------------------------------------
        // UC_AF1 — FILTERING USERS
        // ---------------------------------------------------------
        [Theory]
        [InlineData("Lopez", "")]
        [InlineData("", "Spam")]
        [InlineData("Lopez", "Spam")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF1_FilteringUsers(string surname, string complaintType)
        {
            InitialStepsForBan();
            selectUsers_PO.SearchUsers(surname, complaintType);
            Assert.False(selectUsers_PO.IsNoUsersResultShown());
        }

        // ---------------------------------------------------------
        // UC_AF1 — FILTERING NO RESULTS
        // ---------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF1_Filtering_NoResults()
        {
            InitialStepsForBan();
            selectUsers_PO.SearchUsers("NonExistent", "FakeType");
            Assert.True(selectUsers_PO.IsNoUsersResultShown());
        }

        // ---------------------------------------------------------
        // UC_AF3 — EMPTY SELECTION → BAN NOT AVAILABLE
        // ---------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF3_EmptySelection_BanNotAvailable()
        {
            InitialStepsForBan();
            Assert.True(selectUsers_PO.BanNotAvailable());
        }

        // ---------------------------------------------------------
        // UC_AF4 — MODIFY SELECTION
        // ---------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF4_ModifySelection()
        {
            InitialStepsForBan();

            string userId = "123";
            selectUsers_PO.SearchUsers(userId, "");
            selectUsers_PO.AddUserToSelection(userId);

            selectUsers_PO.RemoveUserFromSelection(userId);

            Assert.True(selectUsers_PO.BanNotAvailable());
        }

        // ---------------------------------------------------------
        // UC_AF5 — VALIDATION ERRORS
        // ---------------------------------------------------------
        [Theory]
        [InlineData("", "Desc", "2026-05-20", "2026-06-20", "The banReason field is required.")]
        [InlineData("Abuse", "", "2026-05-20", "2026-06-20", "The banDescription field is required.")]
        [InlineData("Abuse", "Desc", "", "2026-06-20", "The banStartDate field is required.")]
        [InlineData("Abuse", "Desc", "2026-05-20", "", "The banEndDate field is required.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF5_ValidationErrors(string reason, string desc, string start, string end, string expectedError)
        {
            InitialStepsForBan();
            AddUserAndProceed("123");

            banUser_PO.FillBanForm(reason, desc, start, end);
            banUser_PO.Submit();

            Assert.True(banUser_PO.ErrorDisplayed());
        }
    }
}
