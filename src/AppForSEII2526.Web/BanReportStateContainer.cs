using AppForSEII2526.API.DTOs.BanUserDTOs;

public class BanReportStateContainer
{
    public BanReportForCreateDTO BanReport { get; private set; } = new();

    public void AddCustomer(UserComplaintsDTO user)
    {
        BanReport.Customers.Add(new ReportCustomerForCreateDTO
        {
            CustomerId = user.Id,
            Message = ""
        });
    }

    public void RemoveCustomer(ReportCustomerForCreateDTO customer)
    {
        BanReport.Customers.Remove(customer);
    }

    public void Clear()
    {
        BanReport = new();
    }
}
