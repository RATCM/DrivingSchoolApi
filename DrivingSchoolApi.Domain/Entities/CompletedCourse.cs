using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class CompletedCourse : Entity<CompletedCourseKey>
{
    public required DrivingSchoolKey SchoolId { get; init; }
    public required StudentKey? StudentId { get; init; }
    public required Money Cost { get; init; }
    public required DateTime CompletionDate { get; init; }
    public required CourseCompletionReason Reason { get; init; }
    
    private CompletedCourse() { } // EF
    
    
    public static CompletedCourse Create(
        CompletedCourseKey id,
        DrivingSchoolKey schoolId,
        StudentKey studentId,
        Money cost,
        DateTime completionDate,
        CourseCompletionReason reason)
    {
        return new CompletedCourse
        {
            Id = id,
            SchoolId = schoolId,
            StudentId = studentId,
            Cost = cost,
            CompletionDate = completionDate.ToUniversalTime(),
            Reason = reason
        };
    }
}