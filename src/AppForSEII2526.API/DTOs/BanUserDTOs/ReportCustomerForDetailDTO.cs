namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class ReportCustomerForDetailDTO
    {
        public ReportCustomerForDetailDTO(string customerId, string name, string surname, string? Message)
        {
            CustomerId = customerId;
            Name = name;
            Surname = surname;
            Message = Message;
        }

        public string CustomerId { get; set; }

        [Required, StringLength(60, ErrorMessage = "Name cannot be longer than 60 characters.")]
        public string Name { get; set; }

        [Required, StringLength(60, ErrorMessage = "Surname cannot be longer than 60 characters.")]
        public string Surname { get; set; }

        [StringLength(250, ErrorMessage = "Personal message cannot be longer than 250 characters.")]
        public string? Message { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReportCustomerForDetailDTO dto &&
                   CustomerId == dto.CustomerId &&
                   Name == dto.Name &&
                   Surname == dto.Surname &&
                   Message == dto.Message;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
