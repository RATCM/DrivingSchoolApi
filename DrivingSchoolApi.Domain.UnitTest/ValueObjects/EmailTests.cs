using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class EmailTests
{
    [Test]
    public void Address_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Email(""));
        
        Assert.That(exception.Message, Is.EqualTo("Email cannot be empty"));
    }

    [Test]
    public void Address_CannotBeInvalid()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Email("invalidEmail.com"));
        
        Assert.That(exception.Message, Is.EqualTo("Email must be valid"));
    }

    [Test]
    public void Email_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new Email("valid@Email.com"));
    }
}