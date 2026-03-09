using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class MoneyMapper
{
    extension (Money entity)
    {
        public MoneyDto ToDto()
        {
            return new MoneyDto
            (
                entity.Amount,
                entity.Currency
            );
        }
    }
}
