using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingLesson : Entity<DrivingLessonKey>
{
    public required DrivingSchoolKey SchoolId { get; init; }
    public required DrivingRoute Route { get; init; }
    public required Money Price { get; init; }
    public required InstructorKey InstructorId { get; init; }
    public required StudentKey StudentId { get; init; }

    private DrivingLesson() {}  // EF

    public static DrivingLesson Create(
        DrivingLessonKey id,
        DrivingSchoolKey schoolId,
        DrivingRoute route,
        Money price,
        InstructorKey instructorId,
        StudentKey studentId)
    {
        return new DrivingLesson
        {
            Id = id,
            SchoolId = schoolId,
            Route = route,
            Price = price,
            InstructorId = instructorId,
            StudentId = studentId
        };
    }
}