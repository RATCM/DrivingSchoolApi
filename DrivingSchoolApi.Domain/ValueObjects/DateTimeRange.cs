using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record DateTimeRange : ValueObject
{
    public required DateTime StartDateTime { get; init; }
    public required DateTime EndDateTime { get; init; }
    
    private DateTimeRange() {} // EF
    
    public static DateTimeRange Create(DateTime startDateTime, DateTime endDateTime)
    {
        if (endDateTime < startDateTime) throw new InvalidInputException("StartDateTime must be before EndDateTime");

        return new DateTimeRange
        {
            StartDateTime = startDateTime,
            EndDateTime = endDateTime
        };
    }
}