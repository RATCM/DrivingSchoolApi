using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class CoordinatePointMapper
{
    extension (CoordinatePoint entity)
    {
        public CoordinatePointDto ToDto()
        {
            return new CoordinatePointDto
            (
                entity.Order,
                entity.Latitude,
                entity.Longitude
            );
        }
    }
}
