
namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class BanReportForCreateDTO
    {

        public BanReportForCreateDTO()
        {
            Customers = new List<ReportCustomerForCreateDTO>();
        }

        public BanReportForCreateDTO(string reason,
                                    string detailedDescription,
                                    DateTime startDate,
                                    DateTime endDate,
                                    IList<ReportCustomerForCreateDTO> customers)
        {
            Reason = reason;
            DetailedDescription = detailedDescription;
            StartDate = startDate;
            EndDate = endDate;
            Customers = customers;
        }


        [Required(ErrorMessage = "Reason is required.")]
        [StringLength(200, ErrorMessage = "Reason cannot be longer than 200 characters.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Detailed description is required.")]
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        public string DetailedDescription { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "At least one user must be included.")]
        [MinLength(1, ErrorMessage = "At least one user must be included.")]
        public IList<ReportCustomerForCreateDTO> Customers { get; set; }

        public IList<ReportCustomerForCreateDTO> Users
        {
            get => Customers;
            set => Customers = value;
        }


        public override bool Equals(object? obj)
        {
            return obj is BanReportForCreateDTO dto &&
                   Reason == dto.Reason &&
                   DetailedDescription == dto.DetailedDescription &&
                   StartDate == dto.StartDate &&
                   EndDate == dto.EndDate &&
                   Customers.SequenceEqual(dto.Customers);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



    }
}
