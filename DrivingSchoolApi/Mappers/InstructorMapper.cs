using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class InstructorMapper
{
    extension(Instructor entity)
    {
        public InstructorDto ToDto()
        {
            return new InstructorDto(
                entity.Id,
                entity.SchoolId,
                entity.InstructorName.ToDto(),
                entity.EmailAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                null,
                null
            );
        }
    }
}