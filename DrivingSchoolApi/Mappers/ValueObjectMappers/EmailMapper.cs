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

    extension(EmailDto dto)
    {
        public Email ToDomain()
        {
            return Email.Create(dto.Address);
        }
    }
}
