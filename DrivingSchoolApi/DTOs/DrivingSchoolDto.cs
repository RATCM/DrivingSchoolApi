namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    string Name,
    string Address,
    string PhoneNumber,
    string WebAddress,
    string PackagePrice);
