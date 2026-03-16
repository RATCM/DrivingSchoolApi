using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.DTOs;
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

    extension(DrivingLessonDto dto)
    {
        public DrivingLesson ToDomain()
        {
            return DrivingLesson.Create(
                DrivingLessonKey.Create(dto.Id),
                DrivingSchoolKey.Create(dto.SchoolId),
                dto.Route.ToDomain(),
                dto.Price.ToDomain(),
                InstructorKey.Create(dto.InstructorId),
                StudentKey.Create(dto.StudentId)
            );
        }
    }
}
