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
