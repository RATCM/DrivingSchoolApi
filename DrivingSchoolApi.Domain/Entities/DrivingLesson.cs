using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingLesson : Entity
{
    public Guid SchoolId { get; }
    public Guid InstructorId { get; }
    public Guid StudentId { get; }
    public DrivingRoute Route { get; }
    public Money Price { get; }
    
    //TODO Instructor signature
    //TODO Student signature

    private DrivingLesson() : base(Guid.Empty) {}  // EF
    public DrivingLesson(
        Guid id,
        Guid schoolId,
        DrivingRoute route,
        Money price,
        Guid instructorId,
        Guid studentId) : base(id)
    {
        SchoolId = schoolId;
        Route = route;
        Price = price;
        InstructorId = instructorId;
        StudentId = studentId;
    }
}