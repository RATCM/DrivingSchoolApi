using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class DrivingLessonMapper
{
    extension(DrivingLesson entity)
    {
        public DrivingLessonDto ToDto()
        {
            return new DrivingLessonDto(
                entity.Id,
                entity.SchoolId,
                entity.InstructorId,
                entity.StudentId,
                entity.Route.ToDto(),
                entity.Price.ToDto()
                );
        }
    }
}