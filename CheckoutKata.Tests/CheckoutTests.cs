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
    [TestCase("A", 50, false)]
    [TestCase("B", 30, false)]
    [TestCase("C", 20, false)]
    [TestCase("D", 15, false)]
    [TestCase("A", 55, true)]
    [TestCase("B", 35, true)]
    [TestCase("C", 25, true)]
    [TestCase("D", 20, true)]
    public void Scan_SingleItem_ReturnsCorrectTotalPrice(string item, int expectedTotal, bool bagsRequired)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        checkout.Scan(item);
        checkout.AddBags(bagsRequired);
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
    }

    [Test]
    [TestCase("A", 2, 100, false)] //Two base unit price A's
    [TestCase("A", 3, 130, false)] //Special offer for A's
    [TestCase("B", 2, 45, false)] //Special offer for B's
    [TestCase("B", 3, 75, false)] //Special offer for two B's with one remaining at base unit price
    [TestCase("B", 4, 90, false)] //Duplicate offer for B's
    [TestCase("C", 3, 60, false)] //Multiple base unit price C's
    [TestCase("A", 2, 105, true)] //Two base unit price A's with bags
    [TestCase("A", 3, 135, true)] //Special offer for A's with bags
    [TestCase("B", 2, 50, true)] //Special offer for B's with bags
    [TestCase("B", 3, 80, true)] //Special offer for two B's with one remaining at base unit price with bags
    [TestCase("B", 4, 95, true)] //Duplicate offer for B's with bags
    [TestCase("C", 8, 170, true)] //Multiple base unit price C's with bags
    [TestCase("D", 12, 195, true)] //Multiple base unit price D's with bags
    public void Scan_MultipleMatchingItems_ReturnsCorrectTotalPrice(string item, int quantity, int expectedTotal, bool bagsRequired)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        for (int i = 0; i < quantity; i++)
        {
            checkout.Scan(item);
        }
        checkout.AddBags(bagsRequired);
        var total = checkout.GetTotalPrice();

        //Assert
        Assert.That(total, Is.EqualTo(expectedTotal));
    }

    [Test]
    [TestCase(new string[] { "A", "B", "A" }, 130, false)] //Two base unit price A's and one base unit price B
    [TestCase(new string[] { "A", "B", "C", "D" }, 115, false)] //One of each item at base unit price
    [TestCase(new string[] { "A", "B", "A", "C", "B" }, 165, false)] //Two A's at base price, special offer for B's, one C at base price
    [TestCase(new string[] { "A", "B", "A", "A", "B" }, 175, false)] //Special offer for A's and special offer for B's
    [TestCase(new string[] { "A", "B", "A" }, 135, true)] //Two base unit price A's and one base unit price B with bags
    [TestCase(new string[] { "A", "B", "C", "D" }, 120, true)] //One of each item at base unit price with bags
    [TestCase(new string[] { "A", "B", "A", "C", "B" }, 170, true)] //Two A's at base price, special offer for B's, one C at base price with bags
    [TestCase(new string[] { "A", "B", "A", "A", "B", "C" }, 205, true)] //Special offer for A's and special offer for B's with bags
    public void Scan_MultipleMixedItems_ReturnsCorrectTotalPrice(string[] items, int expectedTotal, bool bagsRequired)
    {
        //Arrange
        var checkout = new Checkout(_pricingRules);

        //Act
        for (int i = 0; i < items.Length; i++)
        {
            checkout.Scan(items[i]);
        }
        checkout.AddBags(bagsRequired);
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
