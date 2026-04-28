using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingSchool;

public record DrivingSchoolUpdateDto(
    string Name,
    StreetAddressDto StreetAddress,
    string PhoneNumber,
    string WebAddress);