using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class DateTimeRangeMapper
{
    extension (DateTimeRange entity)
    {
        public DateTimeRangeDto ToDto()
        {
            return new DateTimeRangeDto
            (
                entity.StartDateTime,
                entity.EndDateTime
            );
        }
    }

    extension(DateTimeRangeDto dto)
    {
        public DateTimeRange ToDomain()
        {
            return DateTimeRange.Create(
                dto.StartDateTime,
                dto.EndDateTime
            );
        }
    }
}
