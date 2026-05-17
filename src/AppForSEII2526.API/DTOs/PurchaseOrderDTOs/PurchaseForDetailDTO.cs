namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseForDetailDTO
    {
        public PurchaseForDetailDTO(int id, decimal totalPrice, DateTime date, string street, string city, string postalCode, string nameSurname, string state, string paymentMethod, string customerUserName, IList<PurchaseItemDTO> items)
        {
            Id = id;
            TotalPrice = totalPrice;
            Date = date;
            Street = street;
            City = city;
            PostalCode = postalCode;
            NameSurname = nameSurname;
            State = state;
            PaymentMethod = paymentMethod;
            CustomerUserName = customerUserName;
            Items = items;
        }

        public PurchaseForDetailDTO(int id, decimal totalPrice, DateTime date, string street, string city, string postalCode, string nameSurname, string state, string paymentMethod, string customerUserName, IList<PurchaseItemDTO> items, int? rating) : this(id, totalPrice, date, street, city, postalCode, nameSurname, state, paymentMethod, customerUserName, items)
        {
            Rating = rating;
        }

        public int Id { get; set; }
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string NameSurname { get; set; } = default!;

        public string State { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;
        public string CustomerUserName { get; set; } = default!;
        public IList<PurchaseItemDTO> Items { get; set; } = new List<PurchaseItemDTO>();

        [Range(0, 5)]
        public int? Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForDetailDTO dTO &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice &&
                   Date == dTO.Date &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   PostalCode == dTO.PostalCode &&
                   NameSurname == dTO.NameSurname &&
                   State == dTO.State &&
                   PaymentMethod == dTO.PaymentMethod &&
                   CustomerUserName == dTO.CustomerUserName &&
                   Items.SequenceEqual(dTO.Items) &&
                   Rating == dTO.Rating;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(TotalPrice);
            hash.Add(Date);
            hash.Add(Street);
            hash.Add(City);
            hash.Add(PostalCode);
            hash.Add(NameSurname);
            hash.Add(State);
            hash.Add(PaymentMethod);
            hash.Add(CustomerUserName);
            hash.Add(Rating);
            foreach (var item in Items)
                hash.Add(item);
            return hash.ToHashCode();
        }
    }
}
