using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.TheoryLesson;

public record TheoryLessonRegistryDto(
    DateTime LessonDateTime,
    MoneyDto Price,
    List<Guid> StudentIds
    //TODO Instructor Signature
    );
    