namespace DrivingSchoolApi.DTOs;

public record TheoryLessonRegistryDto(
    DateTime LessonDateTime,
    MoneyDto Price,
    List<Guid> StudentIds
    //TODO Instructor Signature
    );
    