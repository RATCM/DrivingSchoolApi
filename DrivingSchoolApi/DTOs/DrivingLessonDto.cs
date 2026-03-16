namespace DrivingSchoolApi.DTOs;

public record DrivingLessonDto(
    Guid Id,
    Guid SchoolId,
    Guid InstructorId,
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price);
    