namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolRegistryDto(
    string Name,
    string Address,
    string PhoneNumber,
    string WebAddress,
    string PackagePrice);
