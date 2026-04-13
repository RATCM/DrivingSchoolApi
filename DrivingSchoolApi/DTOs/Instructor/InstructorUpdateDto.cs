using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Instructor;

public record InstructorUpdateDto(
    Guid SchoolId,
    NameDto Name,
    string Email,
    string PhoneNumber);