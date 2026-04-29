using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingSchool;

public sealed record DrivingSchoolRegistryDto(
    string Name,
    StreetAddressDto StreetAddress,
    string PhoneNumber,
    string WebAddress,
    PackageDto[] Packages);
