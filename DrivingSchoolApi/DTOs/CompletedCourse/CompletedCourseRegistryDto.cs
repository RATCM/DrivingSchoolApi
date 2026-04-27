namespace DrivingSchoolApi.DTOs.CompletedCourse;

public sealed record CompletedCourseRegistryDto(
    DateTime IncludeLessonsFrom,
    string Reason);