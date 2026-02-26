namespace DrivingSchoolApi.DTOs;

public record DrivingLessonDto(
    Guid Id,
    Guid InstructorId,
    Guid StudentId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<CoordinateDto> Route,
    string Price);
