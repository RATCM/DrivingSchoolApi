using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.DrivingSchool;

public sealed record DrivingSchoolRatingDto(
    float PassRate,
    float FailRate,
    float QuitRate,
    MoneyDto AveragePrice);
