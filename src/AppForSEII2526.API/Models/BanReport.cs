namespace AppForSEII2526.API.Models
{
    public class BanReport
    {
        public int ID { get; set; }


        [StringLength(100, ErrorMessage = "Description should not be longer than 100 characters")]
        public string DetailedDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [StringLength(50, ErrorMessage = "Reason should not be longer than 50 characters")]
        public string Reason { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        public List<ReportCustomer> ReportCustomers { get; set; }
        public ReportState State { get; set; } = ReportState.InProgress;
        public List<Complaint> Complaints { get; set; } = new();



    }
}
