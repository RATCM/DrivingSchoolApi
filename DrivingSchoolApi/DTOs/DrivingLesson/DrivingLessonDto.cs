using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingLesson;

public record DrivingLessonDto(
    Guid Id,
    Guid SchoolId,
    Guid InstructorId,
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price);
    