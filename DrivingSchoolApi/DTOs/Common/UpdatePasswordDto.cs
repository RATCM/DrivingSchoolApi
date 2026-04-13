namespace DrivingSchoolApi.DTOs.Common;

public record UpdatePasswordDto(
    string OldPassword,
    string NewPassword);