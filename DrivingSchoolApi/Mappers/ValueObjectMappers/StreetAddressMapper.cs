using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

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
}
