using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class PasswordHashTests
{
    [Test]
    public void Hash_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new PasswordHash(""));
        
        Assert.That(exception.Message, Is.EqualTo("Password hash cannot be empty"));
    }

    [Test]
    public void PasswordHash_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new PasswordHash("SOME_HASHED_PASSWORD"));
    }
}