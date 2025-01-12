using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    public class Checkout : ICheckout
    {
        private readonly List<PricingRule> _pricingRules;
        private readonly List<ScannedItem> _scannedItems;

        public Checkout(List<PricingRule> pricingRules)
        {
            _pricingRules = pricingRules;
            _scannedItems = new List<ScannedItem>();
        }

        public int GetTotalPrice()
        {
            int totalPrice = 0;
            foreach (var scannedItem in _scannedItems)
            {
                var pricingRule = _pricingRules.FirstOrDefault(pr => pr.SKU == scannedItem.SKU);
                if (pricingRule != null)
                {
                    totalPrice = scannedItem.Quantity * pricingRule.UnitPrice;
                }
            }
            return totalPrice;
        }

        public void Scan(string item)
        {
            var scannedItem = _scannedItems.FirstOrDefault(si => si.SKU == item);

            if (scannedItem != null)
            {
                scannedItem.Quantity += 1;   
            }
            else
            {
                _scannedItems.Add(new ScannedItem(item));
            }
        }
    }
}
