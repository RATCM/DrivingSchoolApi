using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class DateTimeRangeTests
{
    [Test]
    public void EndDateTime_CannotComeBefore_StartDateTime()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => DateTimeRange.Create(
                new DateTime(2001, 01, 01), 
                new DateTime(2000, 01, 01)));
        
        Assert.That(exception.Message, Is.EqualTo("StartDateTime must be before EndDateTime"));
    }

    [Test]
    public void DateTimeRange_SucceedsWhenValid()
    { 
        Assert.DoesNotThrow(
            () => DateTimeRange.Create(
                new DateTime(2000, 01, 01), 
                new DateTime(2001, 01, 01)));
    }
}