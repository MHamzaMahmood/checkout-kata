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
        Assert.That(total, Is.EqualTo(50));
    }

    [Test]
    public void ScanMultipleItemsWithNoOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("A");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(100));
    }

    [Test]
    public void ScanMultipleItemsWithOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(130));
    }

    [Test]
    public void ScanMultipleMixedItemsWithNoOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(130));
    }
}
