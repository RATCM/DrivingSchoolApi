using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.ValueObject;

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

    extension(MoneyDto dto)
    {
        public Money ToDomain()
        {
            return Money.Create(
                dto.Amount,
                dto.Currency
            );
        }
    }
}
