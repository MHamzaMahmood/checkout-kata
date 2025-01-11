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

        public Checkout(List<PricingRule> pricingRules)
        {
            _pricingRules = pricingRules;
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

        public void Scan(string item)
        {
            throw new NotImplementedException();
        }
    }
}
