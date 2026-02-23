using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class AddressTests
{
    [Test]
    public void PostalCode_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Address("", "city", "region", "addressLine"));
        
        Assert.That(exception.Message, Is.EqualTo("Postal code cannot be empty"));
    }
    
    [Test]
    public void City_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Address("postalCode", "", "region", "addressLine"));
        
        Assert.That(exception.Message, Is.EqualTo("City cannot be empty"));
    }

    [Test]
    public void Region_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Address("postalCode", "city", "", "addressLine"));

        Assert.That(exception.Message, Is.EqualTo("Region cannot be empty"));
    }
    
    [Test]
    public void AddressLine_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Address("postalCode", "city", "region", ""));
        
        Assert.That(exception.Message, Is.EqualTo("Address line cannot be empty"));
    }

    [Test]
    public void Address_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new Address("postalCode", "city", "region", "addressLine"));
    }
}