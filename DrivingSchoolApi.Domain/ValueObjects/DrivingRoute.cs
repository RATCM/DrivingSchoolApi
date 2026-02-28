using System.Collections.Immutable;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record DrivingRoute : ValueObject
{
    public required DateTimeRange DateTimeRange { get; init; }
    public required ImmutableArray<CoordinatePoint> RouteCoordinates { get; init; }

    private DrivingRoute() {} // EF
    
    public static DrivingRoute Create(DateTimeRange dateTimeRange, CoordinatePoint[] routeCoordinates)
    {
        HashSet<int> orderSet = routeCoordinates.Select(x => x.Order).ToHashSet();
        if (orderSet.Count < routeCoordinates.Length)
            throw new InvalidInputException("Route coordinates cannot contain order duplicates");
        if (orderSet.Max() - orderSet.Min() + 1 != routeCoordinates.Length)
            throw new InvalidInputException("Route coordinates order cannot have gaps");
        if (orderSet.Min() != 1)
            throw new InvalidInputException("Route coordinates must start at 1");

        return new DrivingRoute
        {
            DateTimeRange = dateTimeRange,
            RouteCoordinates = [..routeCoordinates]
        };
    }
}