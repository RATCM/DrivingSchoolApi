using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class NameTests
{
    [Test]
    public void FirstName_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Name.Create("", "Last"));
        
        Assert.That(exception.Message, Is.EqualTo("First name cannot be empty"));
    }
    
    [Test]
    public void LastName_CannotBeEmpty()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => Name.Create("First", ""));
        
        Assert.That(exception.Message, Is.EqualTo("Last name cannot be empty"));
    }

    [Test]
    public void Name_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => Name.Create("First", "Last"));
    }
}