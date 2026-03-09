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
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.InstructorId.Value,
                entity.StudentId.Value,
                entity.Route.ToDto(),
                entity.Price.ToDto()
                );
        }
    }
}
