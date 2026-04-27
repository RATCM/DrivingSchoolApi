using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.TheoryLesson;

public record TheoryLessonRegistryDto(
    DateTime LessonDateTime,
    MoneyDto Price,
    Guid StudentId,
    IFormFile InstructorSignature,
    IFormFile StudentSignature
    );
    