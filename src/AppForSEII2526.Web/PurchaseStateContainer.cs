using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public PurchaseForCreateDTO Purchase { get; private set; }

        public event Action? OnChange;

        public PurchaseStateContainer()
        {
            Purchase = new PurchaseForCreateDTO
            {
                Items = new List<PurchaseItemDTO>()
            };
        }

        public double TotalPriceClient => Purchase.Items.Sum(i => i.UnitPrice * i.Quantity);

        public void AddProduct(ProductForPurchaseDTO p, decimal? unitPrice = null)
        {
            var existing = Purchase.Items.FirstOrDefault(i => i.ProductId == p.ProductId);
            if (existing == null)
            {
                Purchase.Items.Add(new PurchaseItemDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Brand = p.Brand,
                    Colour = p.Colour,

                    // El cliente autogenerado define UnitPrice como double, por lo que debemos
                    // hacer un casting explícito desde el decima' original de la API.

                    UnitPrice = unitPrice.HasValue
                        ? (double)unitPrice.Value
                        : (double)p.Price,
                  

                    Quantity = 1
                });
            }
            else
            {
                existing.Quantity += 1;
            }
            Notify();
        }

        public void IncreaseQuantity(PurchaseItemDTO item)
        {
            var existing = Purchase.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += 1;
                Notify();
            }
        }

        public void DecreaseQuantity(PurchaseItemDTO item)
        {
            var existing = Purchase.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing != null)
            {
                if (existing.Quantity > 1)
                {
                    existing.Quantity -= 1;
                }
                else
                {
                    Purchase.Items.Remove(existing);
                }
                Notify();
            }
        }

        public void RemoveItem(PurchaseItemDTO item)
        {
            var existing = Purchase.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing != null)
            {
                Purchase.Items.Remove(existing);
                Notify();
            }
        }

        public void ClearCart()
        {
            Purchase.Items.Clear();
            Notify();
        }

        private void Notify()
        {
            OnChange?.Invoke();
        }
    }
}
