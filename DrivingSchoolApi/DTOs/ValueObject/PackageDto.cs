namespace DrivingSchoolApi.DTOs.ValueObject;

public record PackageDto(
    string Title,
    string Description,
    MoneyDto Price);
