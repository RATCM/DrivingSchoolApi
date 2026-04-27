using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.TheoryLesson;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class TheoryLessonMapper
{
    extension(TheoryLesson entity)
    {
        public TheoryLessonDto ToDto()
        {
            return new TheoryLessonDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.InstructorId?.Value,
                entity.LessonDateTime,
                entity.Price.ToDto(),
                entity.StudentId?.Value
            );
        }
        
    }
}
