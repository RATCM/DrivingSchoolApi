using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record CoordinatePoint : ValueObject
{
    public required int Order { get; init; }
    public required float Latitude { get; init; }
    public required float Longitude { get; init; }
    
    private CoordinatePoint() {} // EF

    public static CoordinatePoint Create(int order, float latitude, float longitude)
    {
        // Validation
        if (order < 1) throw new InvalidInputException("Order cannot be less than 1");
        if (latitude is > 180 or < -180) throw new InvalidInputException("Latitude must be in the range [-180;180]");
        if (longitude is > 180 or < -180) throw new InvalidInputException("Longitude must be in the range [-180;180]");

        return new CoordinatePoint()
        {
            Order = order,
            Latitude = latitude,
            Longitude = longitude
        };
    }
}