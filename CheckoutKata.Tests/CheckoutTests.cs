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
    public void ScanEveryAvailableItemWithNoOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("C");
        checkout.Scan("D");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(115));
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

    [Test]
    public void ScanMultipleMixedItemsWithOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        checkout.Scan("C");
        checkout.Scan("B");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(165));
    }

    [Test]
    public void ScanMultipleMixedItemsWithMultipleOffers()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("B");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(175));
    }

    [Test]
    public void ScanMultipleItemsWithDuplicateOffers()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(90));
    }

    [Test]
    public void ScanMultipleMatchingItemsWithSingleOffer()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("B");
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(75));
    }
}
