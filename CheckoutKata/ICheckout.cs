namespace CheckoutKata
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
        void AddBags(bool bagsRequired);
    }
}