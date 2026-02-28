using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class DrivingSchoolNameMapper
{
    extension (DrivingSchoolName entity)
    {
        public DrivingSchoolNameDto ToDto()
        {
            return new DrivingSchoolNameDto
            (
                entity.Name
            );
        }
    }
}