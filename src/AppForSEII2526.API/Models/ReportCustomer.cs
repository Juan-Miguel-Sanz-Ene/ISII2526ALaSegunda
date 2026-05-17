namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BanReportId),nameof(CustomerId))]
    public class ReportCustomer
    {
        public int BanReportId { get; set; }
        public ReportState State { get; set; }

        public string CustomerId { get; set; }

        [StringLength(100, ErrorMessage = "Message should not be longer than 100 characters")]
        public string? Message { get; set; }

        public BanReport BanReport { get; set; }
      //  public ApplicationUser User { get; set; }
        public ApplicationUser Customer { get;  set; }
    }

    public enum ReportState
    {
        InProgress, Completed
    }
}
