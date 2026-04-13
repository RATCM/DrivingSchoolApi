using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class InstructorMapper
{
    extension(Instructor entity)
    {
        public InstructorDto ToDto()
        {
            return new InstructorDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.InstructorName.ToDto(),
                entity.EmailAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                null,
                null
            );
        }
    }
}
