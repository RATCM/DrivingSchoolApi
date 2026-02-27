using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class WebAddressTests
{
    [Test]
    public void Url_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => WebAddress.Create(""));
        
        Assert.That(exception.Message, Is.EqualTo("Url cannot be empty"));
    }

    [Test]
    public void Url_CannotBeInvalid()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => WebAddress.Create("invalid url"));
        
        Assert.That(exception.Message, Is.EqualTo("Url must be valid"));
    }
}