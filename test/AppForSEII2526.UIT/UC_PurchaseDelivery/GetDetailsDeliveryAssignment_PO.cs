using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class GetDetailsDeliveryAssignment_PO : PageObject
    {
        public GetDetailsDeliveryAssignment_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckDeliveryAssignmentDetail(string id, string deliveryDriverName, string personalMessage,
            string deliveryAssignmentDone, string extraReward)
        {
            WaitForBeingVisible(By.Id("ID"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("ID")).Text.Contains(id);
            result = result && _driver.FindElement(By.Id("DeliveyDriverName")).Text.Contains(deliveryDriverName);
            result = result && _driver.FindElement(By.Id("PersonalMessage")).Text.Contains(personalMessage);
            result = result && _driver.FindElement(By.Id("DeliveryAssignmentDone")).Text.Contains(deliveryAssignmentDone);
            result = result && _driver.FindElement(By.Id("ExtraReward")).Text.Contains(extraReward);

            return result;

        }

        public bool CheckDeliveryAssignmentDetailWithoutID(string deliveryDriverName, string personalMessage,
            string deliveryAssignmentDone, string extraReward)
        {
            WaitForBeingVisible(By.Id("ID"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("DeliveyDriverName")).Text.Contains(deliveryDriverName);
            result = result && _driver.FindElement(By.Id("PersonalMessage")).Text.Contains(personalMessage);
            result = result && _driver.FindElement(By.Id("DeliveryAssignmentDone")).Text.Contains(deliveryAssignmentDone);
            result = result && _driver.FindElement(By.Id("ExtraReward")).Text.Contains(extraReward);

            return result;

        }

        public bool CheckListOfPurchaseDeliveries(List<string[]> expectedPurchaseDeliveries)
        {   WaitForBeingVisible(By.Id("AssignedDeliveries"));
            return CheckBodyTable(expectedPurchaseDeliveries, By.Id("AssignedDeliveries"));
        }

        public bool CheckErrorMessage(string expectedErrorMessage)
        {   
            WaitForBeingVisible(By.TagName("body"));
            return _driver.PageSource.Contains(expectedErrorMessage);
        }
    }
}
