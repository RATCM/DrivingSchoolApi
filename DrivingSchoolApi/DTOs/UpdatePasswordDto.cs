namespace DrivingSchoolApi.DTOs;

public record UpdatePasswordDto(
    string OldPassword,
    string NewPassword);