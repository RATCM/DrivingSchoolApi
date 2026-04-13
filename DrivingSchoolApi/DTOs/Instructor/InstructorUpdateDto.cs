using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Instructor;

public record InstructorUpdateDto(
    NameDto Name,
    string Email,
    string PhoneNumber);