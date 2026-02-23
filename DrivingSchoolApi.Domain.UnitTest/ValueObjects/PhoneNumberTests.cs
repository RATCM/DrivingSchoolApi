using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class PhoneNumberTests
{
    [Test]
    public void Number_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new PhoneNumber(""));
        
        Assert.That(exception.Message, Is.EqualTo("Phone number cannot be empty"));
    }

    [Test]
    public void PhoneNumber_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new PhoneNumber("12345678"));
    }
}