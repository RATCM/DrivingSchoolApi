using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class CoordinatePointTests
{
    [Test]
    public void Order_CannotBeLessThan1()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new CoordinatePoint(0, 1, 2));
        
        Assert.That(exception.Message, Is.EqualTo("Order cannot be less than 1"));
    }

    [Test]
    public void Latitude_CannotBeOutsideOfRange()
    {
        var exception1 = Assert.Throws<InvalidInputException>(
            () => new CoordinatePoint(1, -200, 2));
        var exception2 = Assert.Throws<InvalidInputException>(
            () => new CoordinatePoint(1, 200, 2));
        
        Assert.That(exception1.Message, Is.EqualTo("Latitude must be in the range [-180;180]"));
        Assert.That(exception2.Message, Is.EqualTo("Latitude must be in the range [-180;180]"));
    }

    [Test]
    public void Longitude_CannotBeOutsideOfRange()
    {
        var exception1 = Assert.Throws<InvalidInputException>(
            () => new CoordinatePoint(1, 1, -200));
        var exception2 = Assert.Throws<InvalidInputException>(
            () => new CoordinatePoint(1, 1, 200));
        
        Assert.That(exception1.Message, Is.EqualTo("Longitude must be in the range [-180;180]"));
        Assert.That(exception2.Message, Is.EqualTo("Longitude must be in the range [-180;180]"));
    }

    [Test]
    public void CoordinatePoint_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(() => new CoordinatePoint(1, 1, 2));
    }
}