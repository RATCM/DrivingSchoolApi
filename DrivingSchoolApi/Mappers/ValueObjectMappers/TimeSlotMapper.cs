using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class TimeSlotMapper
{
    extension(TimeSlot timeSlot)
    {
        public TimeSlotDto ToDto()
        {
            return new TimeSlotDto(
                timeSlot.Description, 
                timeSlot.StartDateTime, 
                timeSlot.EndDateTime);
        }
    }
}