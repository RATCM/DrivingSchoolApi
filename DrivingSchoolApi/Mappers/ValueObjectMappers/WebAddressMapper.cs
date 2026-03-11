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
}
