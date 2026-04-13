using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingLesson;

public record DrivingLessonRegistryDto(
    IFormFile InstructorSignature,
    IFormFile StudentSignature,
    Guid SchoolId,
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price
    );
