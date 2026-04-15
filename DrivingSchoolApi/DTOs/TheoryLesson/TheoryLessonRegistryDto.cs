using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.TheoryLesson;

public record TheoryLessonRegistryDto(
    IFormFile InstructorSignature,
    Guid SchoolId,
    List<Guid> StudentIds,
    DateTime LessonDateTime,
    MoneyDto Price
    );
    