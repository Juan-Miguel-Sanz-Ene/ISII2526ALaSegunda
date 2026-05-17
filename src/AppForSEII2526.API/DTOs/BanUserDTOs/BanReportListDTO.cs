namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class BanReportListDTO
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public string DetailedDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
        public List<ReportCustomerDTO> Customers { get; set; }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

}
