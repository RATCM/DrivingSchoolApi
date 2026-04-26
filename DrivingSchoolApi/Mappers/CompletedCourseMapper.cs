using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs.CompletedCourse;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class CompletedCourseMapper
{
    extension(CompletedCourse entity)
    {
        public CompletedCourseDto ToDto()
        {
            return new CompletedCourseDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.Cost.ToDto(),
                entity.CompletionDate,
                entity.Reason.ToString());
        }
    }
}