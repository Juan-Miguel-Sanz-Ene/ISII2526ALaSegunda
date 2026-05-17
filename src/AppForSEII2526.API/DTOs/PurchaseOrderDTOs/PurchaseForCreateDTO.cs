namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseForCreateDTO
    {
        
        public PurchaseForCreateDTO()
        {
            Items = new List<PurchaseItemDTO>();
        }

        public PurchaseForCreateDTO(string street, string city, string postalCode, string nameCustomer, string surnameCustomer, IList<PurchaseItemDTO> items, int paymentMethodId, int? rating)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            NameCustomer = nameCustomer;
            SurnameCustomer = surnameCustomer;
            Items = items;
            PaymentMethodId = paymentMethodId;
            Rating = rating;  // Fix: assign rating parameter
        }

        [Required, StringLength(120, MinimumLength = 3)]
        public string Street { get; set; }

        [Required, StringLength(60, MinimumLength = 2)]
        public string City { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string PostalCode { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string NameCustomer { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string SurnameCustomer { get; set; }

        [Required]
        public IList<PurchaseItemDTO> Items { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Range(0, 5)]
        public int? Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   PostalCode == dTO.PostalCode &&
                   NameCustomer == dTO.NameCustomer &&
                   SurnameCustomer == dTO.SurnameCustomer &&
                   EqualityComparer<IList<PurchaseItemDTO>>.Default.Equals(Items, dTO.Items) &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   Rating == dTO.Rating;
        }
    }

}
