using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Student;

public sealed record StudentRegistryDto(
    NameDto StudentName,
    string EmailAddress, 
    string PhoneNumber,
    string Password,
    Guid InviteId);
    