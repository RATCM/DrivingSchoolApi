using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.DTOs;

public record DrivingLessonRegistry(
    Guid SchoolId,
    Guid InstructorId,
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price
    //TODO Instructor signature
    //TODO Student signature
    );
    