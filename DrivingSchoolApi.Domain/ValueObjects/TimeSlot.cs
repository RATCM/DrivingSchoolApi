using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record TimeSlot : ValueObject
{
    public required string Description { get; init; }
    public required DateTime StartDateTime { get; init; }
    public required DateTime EndDateTime { get; init; }
    
    private TimeSlot() {}

    public static TimeSlot Create(string description, DateTimeRange range)
    {
        if (string.IsNullOrEmpty(description))
            throw new InvalidInputException("TimeSlot description cannot be empty");
        
        return new TimeSlot
        {
            Description = description,
            StartDateTime = range.StartDateTime,
            EndDateTime = range.EndDateTime
        };
    }
}