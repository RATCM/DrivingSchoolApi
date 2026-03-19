namespace DrivingSchoolApi.DTOs;

public record InstructorUpdateDto(
    Guid SchoolId,
    NameDto Name,
    string Email,
    string PhoneNumber);