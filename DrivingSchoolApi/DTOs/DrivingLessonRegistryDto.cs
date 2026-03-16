using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.DTOs;

public record DrivingLessonRegistryDto(
    Guid StudentId,
    DrivingRouteDto Route,
    MoneyDto Price
    //TODO Instructor signature
    //TODO Student signature
    );
