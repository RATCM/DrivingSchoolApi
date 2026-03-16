namespace DrivingSchoolApi.DTOs;

public sealed record DrivingSchoolDto(
    Guid Id,
    DrivingSchoolNameDto Name,
    StreetAddressDto StreetAddress,
    PhoneNumberDto PhoneNumber,
    WebAddressDto WebAddress,
    MoneyDto PackagePrice,
    IReadOnlyList<StudentDto>? Students,
    IReadOnlyList<InstructorDto>? Instructors);
