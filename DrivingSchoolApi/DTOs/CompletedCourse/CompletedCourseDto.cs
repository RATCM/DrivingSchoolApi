using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.CompletedCourse;

public sealed record CompletedCourseDto(
    Guid Id,
    Guid SchoolId,
    MoneyDto Cost,
    DateTime CompletionDate,
    string Reason);