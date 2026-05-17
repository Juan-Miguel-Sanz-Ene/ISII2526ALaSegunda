namespace AppForSEII2526.API.Models
{
    public class Complaint
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ComplaintDate { get; set; }

        [StringLength(100, ErrorMessage = "Description should not be longer than 100 characters")]
        public string Description { get; set; }

        public int ID { get; set; }

        public bool Processed { get; set; }
        public ApplicationUser User { get; set; }
        public ComplaintType Type { get; set; }
        public int? BanReportId { get; set; }
        public BanReport? BanReport { get; set; }

    }
}
