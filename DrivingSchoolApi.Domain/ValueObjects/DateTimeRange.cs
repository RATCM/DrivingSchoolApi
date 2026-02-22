using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record DateTimeRange : ValueObject
{
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    
    private DateTimeRange() {} // EF

    public DateTimeRange(DateTime startDateTime, DateTime endDateTime)
    {
        if (endDateTime < startDateTime) throw new InvalidInputException("StartDateTime must be before EndDateTime");

        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }
}