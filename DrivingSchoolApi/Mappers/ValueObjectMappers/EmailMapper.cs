using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class EmailMapper
{
    extension (Email entity)
    {
        public EmailDto ToDto()
        {
            return new EmailDto(
                entity.Address
            );
        }
    }
}
