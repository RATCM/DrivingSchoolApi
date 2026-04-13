using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.ValueObject;

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
    
    extension (CoordinatePointDto dto)
    {
        public CoordinatePoint ToDomain()
        {
            return CoordinatePoint.Create(
                dto.Order,
                dto.Latitude,
                dto.Longitude
            );
        }
    }
}
