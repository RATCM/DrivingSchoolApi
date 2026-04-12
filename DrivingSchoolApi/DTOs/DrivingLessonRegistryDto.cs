namespace DrivingSchoolApi.DTOs;

public record DrivingLessonRegistryDto(
    IFormFile InstructorSignature,
    IFormFile StudentSignature,
    Guid SchoolId,
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price
    );
