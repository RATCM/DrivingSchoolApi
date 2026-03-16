using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.DTOs;

public record DrivingLessonRegistry(
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price
    //TODO Instructor signature
    //TODO Student signature
    );
