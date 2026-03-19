namespace DrivingSchoolApi.DTOs;

public sealed record StudentRegistryDto(
    Guid SchoolId, 
    NameDto StudentName,
    string EmailAddress, 
    string PhoneNumber,
    string Password,
    Guid InviteId);
    