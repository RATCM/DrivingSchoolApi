using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class DrivingRouteTests
{
    [Test]
    public void RouteCoordinates_CannotContainOrderDuplicates()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new DrivingRoute(
                new DateTimeRange(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [new CoordinatePoint(1, 1, 2), new CoordinatePoint(1, 2, 3)]
        ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates cannot contain order duplicates"));
    }

    [Test]
    public void RouteCoordinates_CannotContainOrderGaps()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new DrivingRoute(
                new DateTimeRange(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [new CoordinatePoint(1, 1, 2), new CoordinatePoint(3, 2, 3)]
            ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates order cannot have gaps"));
    }

    [Test]
    public void RouteCoordinates_MustStartFromOrder1()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => new DrivingRoute(
                new DateTimeRange(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [new CoordinatePoint(2, 1, 2), new CoordinatePoint(3, 2, 3)]
            ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates must start at 1"));
    }

    [Test]
    public void RouteCoordinates_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(
            () => new DrivingRoute(
                new DateTimeRange(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [new CoordinatePoint(1, 1, 2), new CoordinatePoint(2, 2, 3)]
            ));
    }
}