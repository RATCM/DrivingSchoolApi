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
                [..entity.RouteCoordinates.Select(x => x.ToDto())]
            );
        }
    }

    extension(DrivingRouteDto dto)
    {
        public DrivingRoute ToDomain()
        {
            return DrivingRoute.Create(
                dto.DateTimeRange.ToDomain(),
                dto.RouteCoordinates.Select(x => x.ToDomain()).ToArray()
            );
        }
    }
}
