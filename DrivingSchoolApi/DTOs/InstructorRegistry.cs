namespace DrivingSchoolApi.DTOs;

public sealed record InstructorRegistry(
    Guid SchoolId,
    string Name,
    string EmailAddress,
    string PhoneNumber);
