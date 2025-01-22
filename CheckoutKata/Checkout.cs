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
        private bool _bagsRequired = false;

        public Checkout(List<PricingRule> pricingRules)
        {
            _pricingRules = pricingRules;
            _scannedItems = new List<ScannedItem>();
        }

        public void AddBags(bool bagsRequired)
        {
            _bagsRequired = bagsRequired;
        }

        public int GetTotalPrice()
        {
            int totalPrice = 0;

            if (_scannedItems.Count == 0)
            {
                throw new ArgumentException("Please scan at least one item", nameof(_scannedItems));
            }

            foreach (var scannedItem in _scannedItems)
            {
                var pricingRule = _pricingRules.FirstOrDefault(pr => pr.SKU == scannedItem.SKU);

                if (pricingRule != null)
                {
                    if (pricingRule.SpecialQuantity != null 
                        && pricingRule.SpecialPrice != null
                        && scannedItem.Quantity >= pricingRule.SpecialQuantity.Value)
                    {
                        int qualifyingItems = scannedItem.Quantity / pricingRule.SpecialQuantity.Value;
                        int remainingItems = scannedItem.Quantity % pricingRule.SpecialQuantity.Value;

                        totalPrice += qualifyingItems * pricingRule.SpecialPrice.Value;
                        totalPrice += remainingItems * pricingRule.UnitPrice;
                    }
                    else
                    {
                        totalPrice += scannedItem.Quantity * pricingRule.UnitPrice;
                    }
                }
            }

            if (_bagsRequired == true)
            {
                int bagsRequired = 0;
                int totalItemCount = _scannedItems.Sum(si => si.Quantity);

                if (totalItemCount > 5)
                {
                    int fullBags = totalItemCount / 5;
                    int remainingItems = totalItemCount % 5;

                    bagsRequired += fullBags;
                    if (remainingItems > 0)
                    {
                        bagsRequired += 1;
                    }
                }
                else
                {
                    bagsRequired = 1;
                }
                totalPrice += bagsRequired * 5;
            }

            return totalPrice;
        }

        public void Scan(string item)
        {
            var scannedItem = _scannedItems.FirstOrDefault(si => si.SKU == item);

            if (!_pricingRules.Any(pr => pr.SKU == item) || string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException("Please provide a valid SKU", nameof(item));
            }

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
