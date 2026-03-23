namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    string Name,
    StreetAddressDto StreetAddress,
    string PhoneNumber,
    string WebAddress,
    IReadOnlyList<PackageDto> Packages,
    IReadOnlyList<StudentDto>? Students = null,
    IReadOnlyList<InstructorDto>? Instructors = null);
