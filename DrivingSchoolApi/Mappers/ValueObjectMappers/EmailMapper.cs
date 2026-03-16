using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class EmailMapper
{
    extension (Email entity)
    {
        public String ToDto()
        {
            return entity.Address.ToString();
        }
    }
}
