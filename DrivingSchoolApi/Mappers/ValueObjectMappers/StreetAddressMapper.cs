using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class StreetAddressMapper
{
    extension(StreetAddress entity)
    {
        public StreetAddressDto ToDto()
        {
            return new StreetAddressDto(
                entity.PostalCode,
                entity.City,
                entity.Region,
                entity.AddressLine
                );
        }
    }

    extension(StreetAddressDto dto)
    {
        public StreetAddress ToDomain()
        {
            return StreetAddress.Create(
                dto.PostalCode,
                dto.City,
                dto.Region,
                dto.AddressLine
            );
        }
    }
}
