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
        return new TimeSlot
        {
            Description = description,
            StartDateTime = range.StartDateTime,
            EndDateTime = range.EndDateTime
        };
    }
}