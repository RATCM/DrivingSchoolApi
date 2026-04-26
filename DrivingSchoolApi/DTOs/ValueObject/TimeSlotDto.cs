namespace DrivingSchoolApi.DTOs.ValueObject;

public sealed record TimeSlotDto(
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime);
