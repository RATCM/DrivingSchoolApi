using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.ValueObjects;

public class DrivingRouteTests
{
    [Test]
    public void RouteCoordinates_CannotContainOrderDuplicates()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => DrivingRoute.Create(
                DateTimeRange.Create(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [CoordinatePoint.Create(1, 1, 2), CoordinatePoint.Create(1, 2, 3)]
        ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates cannot contain order duplicates"));
    }

    [Test]
    public void RouteCoordinates_CannotContainOrderGaps()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => DrivingRoute.Create(
                DateTimeRange.Create(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [CoordinatePoint.Create(1, 1, 2), CoordinatePoint.Create(3, 2, 3)]
            ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates order cannot have gaps"));
    }

    [Test]
    public void RouteCoordinates_MustStartFromOrder1()
    {
        var exception = Assert.Throws<InvalidInputException>(
            () => DrivingRoute.Create(
                DateTimeRange.Create(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [CoordinatePoint.Create(2, 1, 2), CoordinatePoint.Create(3, 2, 3)]
            ));
        
        Assert.That(exception.Message, Is.EqualTo("Route coordinates must start at 1"));
    }

    [Test]
    public void RouteCoordinates_SucceedsWhenValid()
    {
        Assert.DoesNotThrow(
            () => DrivingRoute.Create(
                DateTimeRange.Create(
                    new DateTime(2000, 01, 01), 
                    new DateTime(2001, 01, 01)), 
                [CoordinatePoint.Create(1, 1, 2), CoordinatePoint.Create(2, 2, 3)]
            ));
    }
}