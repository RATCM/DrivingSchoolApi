namespace DrivingSchoolApi.DTOs;

public record InstructorLoginRequestDto(
    string Username,
    string Password);