using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class WebAddressMapper
{
    extension(WebAddress entity)
    {
        public WebAddressDto ToDto()
        {
            return new WebAddressDto(
                entity.Url
            );
        }
    }

    extension(WebAddressDto dto)
    {
        public WebAddress ToDomain()
        {
            return WebAddress.Create(
                dto.Url
            );
        }
    }
}
