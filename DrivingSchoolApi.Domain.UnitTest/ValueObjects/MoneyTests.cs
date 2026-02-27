using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class MoneyTests
{
    [Test]
    public void Amount_CannotBeNegative()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Money.Create(-1, "USD"));
        
        Assert.That(exception.Message, Is.EqualTo("Money amount cannot be negative"));
    }

    [Test]
    public void Currency_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Money.Create(1, ""));
        
        Assert.That(exception.Message, Is.EqualTo("Currency cannot be null or empty"));
    }
    
    [Test]
    public void Money_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => Money.Create(1, "USD"));
    }
}