

using Mono.TextTemplating;
using System.Drawing;

namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class BanDetailDTO
    {
        private object value;

        public BanDetailDTO(int id,
            string reason,
            string detailedDescription,
            DateTime startDate,
            DateTime endDate,
            string state,
            IList<ReportCustomerForDetailDTO> reportedUsers){

            Id = id;
            Reason = reason;
            DetailedDescription = detailedDescription;
            StartDate = startDate;
            EndDate = endDate;
            State = state;
            ReportedUsers = reportedUsers;
        }


        public int Id { get; set; }

        [Required, StringLength(200, ErrorMessage = "Reason cannot be longer than 200 characters.")]
        public string Reason { get; set; }

        [Required, StringLength(500, ErrorMessage = "Detailed description cannot be longer than 500 characters.")]
        public string DetailedDescription { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(20, ErrorMessage = "State cannot be longer than 20 characters.")]
        public string State { get; set; }

        public IList<ReportCustomerForDetailDTO> ReportedUsers { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BanDetailDTO dto &&
                   Id == dto.Id &&
                   Reason == dto.Reason &&
                   DetailedDescription == dto.DetailedDescription &&
                   StartDate == dto.StartDate &&
                   EndDate == dto.EndDate &&
                   State == dto.State &&
                   ReportedUsers.SequenceEqual(dto.ReportedUsers);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
