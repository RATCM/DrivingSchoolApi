using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class StudentMapper
{
    extension(Student entity)
    {
        public StudentDto ToDto(IEnumerable<DrivingLesson>? drivingLessons = null, IEnumerable<TheoryLesson>? theoryLessons = null)
        {
            return new StudentDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.StudentName.ToDto(),
                entity.EmailAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                theoryLessons?.Select(x=>x.ToDto()).ToList(),
                drivingLessons?.Select(x=>x.ToDto()).ToList()
            );
        }
        
    }
}
