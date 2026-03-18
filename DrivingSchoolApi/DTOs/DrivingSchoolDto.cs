namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    String Name,
    StreetAddressDto StreetAddress,
    String PhoneNumber,
    String WebAddress,
    IReadOnlyList<PackageDto> Packages,
    IReadOnlyList<StudentDto>? Students = null,
    IReadOnlyList<InstructorDto>? Instructors = null);
