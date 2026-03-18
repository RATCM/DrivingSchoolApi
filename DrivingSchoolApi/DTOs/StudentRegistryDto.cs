namespace DrivingSchoolApi.DTOs;

public sealed record StudentRegistryDto(
    Guid SchoolId, 
    NameDto StudentName,
    String EmailAddress, 
    String PhoneNumber,
    string Password,
    Guid InviteId);
    