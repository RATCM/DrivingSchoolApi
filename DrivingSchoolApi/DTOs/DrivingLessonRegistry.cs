namespace DrivingSchoolApi.DTOs;

public record DrivingLessonRegistry(
    Guid SchoolId,
    Guid InstructorId,
    Guid StudentId,
    List<CoordinatePointDto> Route,
    string Price
    //TODO Instructor signature
    //TODO Student signature
    );
    