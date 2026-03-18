using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class PhoneNumberMapper
{
    extension (PhoneNumber entity) 
    {
        public String ToDto()
        {
            return entity.Number.ToString();
        }
    }
}
