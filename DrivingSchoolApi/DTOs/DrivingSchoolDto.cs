namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    DrivingSchoolNameDto Name,
    StreetAddressDto StreetAddress,
    PhoneNumberDto PhoneNumber,
    WebAddressDto WebAddress,
    IReadOnlyList<PackageDto> Packages,
    IReadOnlyList<StudentDto>? Students,
    IReadOnlyList<InstructorDto>? Instructors);
