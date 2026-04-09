namespace DrivingSchoolApi.DTOs;

public record StudentLoginRequestDto(
    string Email,
    string Password);