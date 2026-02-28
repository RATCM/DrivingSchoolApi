using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class PhoneNumberMapper
{
    extension (PhoneNumber entity) 
    {
        public PhoneNumberDto ToDto()
        {
            return new PhoneNumberDto
            (
                entity.Number
            ); 
        }
    }
}