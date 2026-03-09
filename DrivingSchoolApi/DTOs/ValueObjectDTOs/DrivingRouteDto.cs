using System.Collections.Immutable;

namespace DrivingSchoolApi.DTOs;

public record DrivingRouteDto(
    DateTimeRangeDto DateTimeRange,
    ImmutableArray<CoordinatePointDto> RouteCoordinates);
    