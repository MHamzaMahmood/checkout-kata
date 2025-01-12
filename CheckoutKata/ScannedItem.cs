using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    public class ScannedItem
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }

        public ScannedItem(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            { 
                throw new ArgumentNullException("Please provide a valid SKU", nameof(sku)); 
            }
            SKU = sku;
            Quantity = 1;
        }
    }
}
