using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Student;

public sealed record StudentCalenderDto(
    string StudentId,
    List<TimeSlotDto> TimeSlots);
