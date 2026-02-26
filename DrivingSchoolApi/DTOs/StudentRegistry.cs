namespace DrivingSchoolApi.DTOs;

public sealed record StudentDtoRegistry(
    Guid SchoolId, 
    string StudentName, 
    string EmailAddress, 
    string PhoneNumber);