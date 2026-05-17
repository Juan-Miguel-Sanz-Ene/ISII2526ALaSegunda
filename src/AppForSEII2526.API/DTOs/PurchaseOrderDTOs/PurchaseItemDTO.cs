namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseItemDTO
    {
        public PurchaseItemDTO(int productId, string name, string brand, string? colour, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            Name = name;
            Brand = brand;
            Colour = colour;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public int ProductId { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Brand { get; set; }

        [StringLength(30)]
        public string? Colour { get; set; }

        [Precision(10, 2)]
        public decimal UnitPrice { get; set; }   // snapshot de Products.Price

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItemDTO dTO &&
                   ProductId == dTO.ProductId &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Colour == dTO.Colour &&
                   UnitPrice == dTO.UnitPrice &&
                   Quantity == dTO.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, Name, Brand, Colour, UnitPrice, Quantity);
        }
    }
}
