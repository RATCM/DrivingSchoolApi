namespace DrivingSchoolApi.DTOs;

public record InstructorLoginRequestDto(
    String Username,
    String Password);