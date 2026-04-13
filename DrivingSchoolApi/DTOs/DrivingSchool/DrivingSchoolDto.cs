using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingSchool ;

public sealed record DrivingSchoolDto(
    Guid Id,
    string Name,
    StreetAddressDto StreetAddress,
    string PhoneNumber,
    string WebAddress,
    IReadOnlyList<PackageDto> Packages,
    IReadOnlyList<StudentDto>? Students = null,
    IReadOnlyList<InstructorDto>? Instructors = null);
