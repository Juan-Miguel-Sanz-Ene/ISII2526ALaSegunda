namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class ReportCustomerForCreateDTO
    {
        public ReportCustomerForCreateDTO() { }

        public ReportCustomerForCreateDTO(string customerId, string? message)
        {
            CustomerId = customerId;
            Message = message;
        }

        [Required]
        public string CustomerId { get; set; }

        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters.")]
        public string? Message { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReportCustomerForCreateDTO dto &&
                   CustomerId == dto.CustomerId &&
                   Message == dto.Message;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }





    }
}
