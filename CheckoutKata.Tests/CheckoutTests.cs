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
    [TestCase("A", 50)]
    [TestCase("B", 30)]
    [TestCase("C", 20)]
    [TestCase("D", 15)]
    public void ScanSingleItem(string item, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan(item);
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
    }

    [Test]
    [TestCase("A", 2, 100)]
    [TestCase("A", 3, 130)]
    [TestCase("B", 2, 45)]
    [TestCase("B", 3, 75)]
    [TestCase("B", 4, 90)]
    [TestCase("C", 3, 60)]
    public void ScanMultipleMatchingItems(string item, int quantity, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        for (int i = 0; i < quantity; i++)
        {
            checkout.Scan(item);
        }
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
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
    [TestCase("")]
    [TestCase("Z")]
    public void ScanInvalidItemSKU(string invalidSku)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);
        var expectedMessage = "Please provide a valid SKU";

        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => checkout.Scan(invalidSku));
        Assert.That(exception.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void GetTotalOfEmptyBasket()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);
        var expectedMessage = "Please scan at least one item";

        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => checkout.GetTotalPrice());
        Assert.That(exception.Message, Does.Contain(expectedMessage));
    }
}
