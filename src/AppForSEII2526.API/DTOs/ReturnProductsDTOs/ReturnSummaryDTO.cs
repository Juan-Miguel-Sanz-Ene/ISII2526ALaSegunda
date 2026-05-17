
namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{
    public class ReturnSummaryDTO
    {
        public ReturnSummaryDTO(
            
            
            
 
            string customerName,
            string customerSurname,
            string customerAddress,
            string customerPhoneNumber,
            string paymentMethod,
            List<ReturnedProductInfoDTO> returnedProducts
        )
        {
       

            CustomerName = customerName;
            CustomerSurname = customerSurname;
            CustomerAddress = customerAddress;
            CustomerPhoneNumber = customerPhoneNumber;
            PaymentMethod = paymentMethod;

            ReturnedProducts = returnedProducts;
        }

        public string PaymentMethod { get; set; }
        
        
        

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public List<ReturnedProductInfoDTO> ReturnedProducts { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReturnSummaryDTO dTO &&
                   PaymentMethod == dTO.PaymentMethod &&
                   CustomerName == dTO.CustomerName &&
                   CustomerSurname == dTO.CustomerSurname &&
                   CustomerAddress == dTO.CustomerAddress &&
                   CustomerPhoneNumber == dTO.CustomerPhoneNumber &&
                   
                   ReturnedProducts.SequenceEqual(dTO.ReturnedProducts);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentMethod, CustomerName, CustomerSurname, CustomerAddress, CustomerPhoneNumber, ReturnedProducts);
        }
    }
}
