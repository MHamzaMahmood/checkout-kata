using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    public class ScannedItem
    {
        public string SKU { get; }
        public int Quantity { get; set; }

        public ScannedItem(string sku)
        {
            SKU = sku;
            Quantity = 1;
        }
    }
}
