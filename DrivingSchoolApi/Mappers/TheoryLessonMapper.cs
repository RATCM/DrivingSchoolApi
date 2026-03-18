using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class TheoryLessonMapper
{
    extension(TheoryLesson entity)
    {
        public TheoryLessonDto ToDto(IEnumerable<StudentKey>? studentIds = null)
        {
            return new TheoryLessonDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.InstructorId.Value,
                entity.LessonDateTime,
                entity.Price.ToDto(),
                studentIds?.Select(x=> x.Value).ToList()
            );
        }
        
    }
    
    extension(TheoryLessonDto dto)
    {
        public TheoryLesson ToDomain()
        {
            return TheoryLesson.Create(
                TheoryLessonKey.Create(dto.Id),
                DrivingSchoolKey.Create(dto.SchoolId),
                dto.LessonDateTime,
                Money.Create(dto.Price.Amount, dto.Price.Currency),
                InstructorKey.Create(dto.InstructorId),
                dto.StudentIds.Select(x=> StudentKey.Create(x)).ToList()
            );
        }
    }
}
