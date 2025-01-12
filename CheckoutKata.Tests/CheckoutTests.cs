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
    public void Scan_SingleItem_ReturnsCorrectTotalPrice(string item, int expectedTotal)
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
    [TestCase("A", 2, 100)] //Two base unit price A's
    [TestCase("A", 3, 130)] //Special offer for A's
    [TestCase("B", 2, 45)] //Special offer for B's
    [TestCase("B", 3, 75)] //Special offer for two B's with one remaining at base unit price
    [TestCase("B", 4, 90)] //Duplicate offer for B's
    [TestCase("C", 3, 60)] //Multiple base unit price C's
    public void Scan_MultipleMatchingItems_ReturnsCorrectTotalPrice(string item, int quantity, int expectedTotal)
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
    [TestCase(new string[] { "A", "B", "A" }, 130)] //Two base unit price A's and one base unit price B
    [TestCase(new string[] { "A", "B", "C", "D" }, 115)] //One of each item at base unit price
    [TestCase(new string[] { "A", "B", "A", "C", "B" }, 165)] //Two A's at base price, special offer for B's, one C at base price
    [TestCase(new string[] { "A", "B", "A", "A", "B" }, 175)] //Special offer for A's and special offer for B's
    public void Scan_MultipleMixedItems_ReturnsCorrectTotalPrice(string[] items, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        for (int i = 0; i < items.Length; i++)
        {
            checkout.Scan(items[i]);
        }
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
    }

    [Test]
    [TestCase("")]
    [TestCase("Z")]
    public void Scan_InvalidItemSKU_ThrowsArgumentExceptionWithCorrectMessage(string invalidSku)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);
        var expectedMessage = "Please provide a valid SKU";

        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => checkout.Scan(invalidSku));
        Assert.That(exception.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void GetTotalPrice_WhenBasketIsEmpty_ThrowsArgumentExceptionWithCorrectMessage()
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);
        var expectedMessage = "Please scan at least one item";

        //Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => checkout.GetTotalPrice());
        Assert.That(exception.Message, Does.Contain(expectedMessage));
    }
}
