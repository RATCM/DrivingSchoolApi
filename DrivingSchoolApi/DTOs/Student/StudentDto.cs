using DrivingSchoolApi.DTOs.DrivingLesson;
using DrivingSchoolApi.DTOs.TheoryLesson;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Student;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    NameDto StudentName, 
    string EmailAddress, 
    string PhoneNumber, 
    List<TheoryLessonDto>? TheoryLessons = null, 
    List<DrivingLessonDto>? DrivingLessons = null);
 