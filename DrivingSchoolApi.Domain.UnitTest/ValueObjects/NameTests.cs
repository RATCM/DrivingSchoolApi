using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class NameTests
{
    [Test]
    public void FirstName_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Name("", "Last"));
        
        Assert.That(exception.Message, Is.EqualTo("First name cannot be empty"));
    }
    
    [Test]
    public void LastName_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new Name("First", ""));
        
        Assert.That(exception.Message, Is.EqualTo("Last name cannot be empty"));
    }

    [Test]
    public void Name_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new Name("First", "Last"));
    }
}