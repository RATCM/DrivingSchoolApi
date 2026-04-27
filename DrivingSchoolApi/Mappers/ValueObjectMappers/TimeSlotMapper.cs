using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class TimeSlotMapper
{
    extension (TimeSlot entity)
    {
        public TimeSlotDto ToDto()
        {
            return new TimeSlotDto
            (
                entity.Description,
                entity.StartDateTime,
                entity.EndDateTime
            ); 
        }
    }

    extension(TimeSlotDto dto)
    {
        public TimeSlot ToDomain()
        {
            return TimeSlot.Create(
                dto.Description,
                DateTimeRange.Create(
                    dto.StartDateTime,
                    dto.EndDateTime)
            );
        }
    }
}