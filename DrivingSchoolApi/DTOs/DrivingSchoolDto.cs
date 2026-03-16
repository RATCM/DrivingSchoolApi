namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    String Name,
    StreetAddressDto StreetAddress,
    String PhoneNumber,
    String WebAddress,
    MoneyDto PackagePrice,
    IReadOnlyList<StudentDto>? Students,
    IReadOnlyList<InstructorDto>? Instructors);
