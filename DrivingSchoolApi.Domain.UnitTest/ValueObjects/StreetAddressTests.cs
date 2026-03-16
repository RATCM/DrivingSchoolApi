using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class StreetAddressTests
{
    [Test]
    public void PostalCode_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => StreetAddress.Create("", "city", "region", "addressLine"));
        
        Assert.That(exception.Message, Is.EqualTo("Postal code cannot be empty"));
    }
    
    [Test]
    public void City_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => StreetAddress.Create("postalCode", "", "region", "addressLine"));
        
        Assert.That(exception.Message, Is.EqualTo("City cannot be empty"));
    }

    [Test]
    public void Region_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => StreetAddress.Create("postalCode", "city", "", "addressLine"));

        Assert.That(exception.Message, Is.EqualTo("Region cannot be empty"));
    }
    
    [Test]
    public void AddressLine_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => StreetAddress.Create("postalCode", "city", "region", ""));
        
        Assert.That(exception.Message, Is.EqualTo("Street Address line cannot be empty"));
    }

    [Test]
    public void Address_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => StreetAddress.Create("postalCode", "city", "region", "addressLine"));
    }
}