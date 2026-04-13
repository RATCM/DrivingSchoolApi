namespace DrivingSchoolApi.DTOs;

public record InstructorUpdateDto(
    NameDto Name,
    string Email,
    string PhoneNumber);