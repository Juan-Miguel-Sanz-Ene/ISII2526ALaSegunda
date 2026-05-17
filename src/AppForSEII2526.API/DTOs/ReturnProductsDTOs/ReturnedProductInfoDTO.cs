
namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{

}
namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{
    public class ReturnedProductInfoDTO
    {
        public ReturnedProductInfoDTO(
            int quantity,
            
            string productName,
            string brandName,
            string warehouseLocation
            
        )
        {
            ProductName = productName;
            BrandName = brandName;
            WarehouseLocation = warehouseLocation;
            Quantity = quantity;
        }

        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string WarehouseLocation { get; set; }
        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReturnedProductInfoDTO dTO &&
                   ProductName == dTO.ProductName &&
                   BrandName == dTO.BrandName &&
                   WarehouseLocation == dTO.WarehouseLocation &&
                   Quantity == dTO.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductName, BrandName, WarehouseLocation, Quantity);
        }
    }
}
