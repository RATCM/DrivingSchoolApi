using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class NameMapper
{
    extension (Name entity)
    {
        public NameDto ToDto()
        {
            return new NameDto
            (
                entity.FirstName,
                entity.LastName
            ); 
        }
    }

    extension(NameDto dto)
    {
        public Name ToDomain()
        {
            return Name.Create(
                dto.FirstName,
                dto.LastName
            );
        }
    }
}
