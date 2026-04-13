namespace DrivingSchoolApi.DTOs.ValueObject;

public record DrivingRouteDto(
    DateTimeRangeDto DateTimeRange,
    CoordinatePointDto[] RouteCoordinates);
    