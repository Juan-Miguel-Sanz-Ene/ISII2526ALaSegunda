namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class ReportCustomerDTO
    {
        public string CustomerId { get; set; }
        public string Message { get; set; }
        public string State { get; set; }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}