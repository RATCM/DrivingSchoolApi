using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class DrivingSchoolNameTests
{
    [Test]
    public void Create_Fails_WhenNameIsEmpty()
    {
        Assert.Throws<InvalidInputException>(() => DrivingSchoolName.Create(""));
    }

    [Test]
    public void Create_Succeeds_WhenNameIsNotEmpty()
    {
        Assert.DoesNotThrow(() => DrivingSchoolName.Create("A name"));
    }
}