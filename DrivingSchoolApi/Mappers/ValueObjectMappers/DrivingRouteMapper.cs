using System.Collections.Immutable;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class DrivingRouteMapper
{
    extension(DrivingRoute entity)
    {
        public DrivingRouteDto ToDto()
        {
            return new DrivingRouteDto(
                entity.DateTimeRange.ToDto(),
                entity.RouteCoordinates.Select(x => x.ToDto()).ToImmutableArray()
            );
        }
    }
}
    