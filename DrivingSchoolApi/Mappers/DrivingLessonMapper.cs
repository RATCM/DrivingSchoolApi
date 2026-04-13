using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.DrivingLesson;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

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
