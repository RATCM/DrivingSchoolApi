namespace DrivingSchoolApi.DTOs;

public record InstructorLoginRequestDto(
    string Email,
    string Password);