using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class EmailTests
{
    [Test]
    public void Address_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Email.Create(""));
        
        Assert.That(exception.Message, Is.EqualTo("Email cannot be empty"));
    }

    [Test]
    public void Address_CannotBeInvalid()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Email.Create("invalidEmail.com"));
        
        Assert.That(exception.Message, Is.EqualTo("Email must be valid"));
    }

    [Test]
    public void Email_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => Email.Create("valid@Email.com"));
    }
}