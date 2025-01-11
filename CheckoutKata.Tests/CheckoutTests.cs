namespace CheckoutKata.Tests;

public class CheckoutTests
{
    private List<PricingRule> _pricingRules;

    [SetUp]
    public void Setup()
    {
        _pricingRules = new List<PricingRule>
        {
            new PricingRule("A", 50, 3, 130),
            new PricingRule("B", 30, 2, 45),
            new PricingRule("C", 20),
            new PricingRule("D", 15)
        };
    }

    [Test]
    public void ScanSingleItem()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.AreEqual(50, total);
    }
}
