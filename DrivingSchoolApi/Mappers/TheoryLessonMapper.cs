using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class TheoryLessonMapper
{
    extension(TheoryLesson entity)
    {
        public TheoryLessonDto ToDtoUnprivileged()
        {
            return new TheoryLessonDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.InstructorId.Value,
                entity.LessonDateTime,
                entity.Price.ToDto(),
                null
            );
        }
        
        //public TheoryLessonDto ToDtoPrivileged()
        //{
        //    return new TheoryLessonDto(
        //        entity.Id.Value,
        //        entity.SchoolId.Value,
        //        entity.InstructorId.Value,
        //        entity.LessonDateTime,
        //        entity.Price.ToDto(),
        //        entity.Students.Select(s => s.ToDtoUnprivileged()).ToList()
        //    );
        //}
    }
}