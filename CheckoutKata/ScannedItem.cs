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
