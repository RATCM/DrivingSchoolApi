using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Student;

public record StudentUpdateDto(
    NameDto Name,
    string Email,
    string PhoneNumber);