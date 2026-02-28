namespace DrivingSchoolApi.DTOs;

public record TheoryLessonDto(
    Guid Id,
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    MoneyDto Price,
    List<StudentDto>? Students);