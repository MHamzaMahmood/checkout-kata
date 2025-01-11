using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    public class PricingRule
    {
        public string SKU { get; }
        public int UnitPrice { get; }
        public int? SpecialQuantity { get; }
        public int? SpecialPrice { get; }

        public PricingRule(string sku, int unitPrice, int? specialQuantity = null, int? specialPrice = null) 
        {
            SKU = sku;
            UnitPrice = unitPrice;
            SpecialQuantity = specialQuantity;
            SpecialPrice = specialPrice;
        }
    }
}
