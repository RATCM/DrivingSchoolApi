using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
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
                entity.InstructorId?.Value,
                entity.StudentId?.Value,
                entity.Route.ToDto(),
                entity.Price.ToDto(),
                entity.CompletedObjective.ToDto()
                );
        }
    }

    extension(DrivingLessonObjective objective)
    {
        public DrivingLessonObjectiveDto ToDto()
        {
            return new DrivingLessonObjectiveDto(
                objective.HasFlag(DrivingLessonObjective.RightOfWay),
                objective.HasFlag(DrivingLessonObjective.Highway),
                objective.HasFlag(DrivingLessonObjective.Night),
                objective.HasFlag(DrivingLessonObjective.ThreePointTurn),
                objective.HasFlag(DrivingLessonObjective.ReverseAroundCorner),
                objective.HasFlag(DrivingLessonObjective.ParallelParking)
            );
        }
    }

    extension(DrivingLessonObjectiveDto objectiveDto)
    {
        public DrivingLessonObjective ToDomain()
        {
            return
                (objectiveDto.RightOfWay ? DrivingLessonObjective.RightOfWay : 0) |
                (objectiveDto.Highway ? DrivingLessonObjective.Highway : 0) |
                (objectiveDto.Night ? DrivingLessonObjective.Night : 0) |
                (objectiveDto.ThreePointTurn ? DrivingLessonObjective.ThreePointTurn : 0) |
                (objectiveDto.ReverseAroundCorner ? DrivingLessonObjective.ReverseAroundCorner : 0) |
                (objectiveDto.ParallelParking ? DrivingLessonObjective.ParallelParking : 0);
        }
    }
}
