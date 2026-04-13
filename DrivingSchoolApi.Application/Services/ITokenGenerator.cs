using DrivingSchoolApi.Application.Enums;

namespace DrivingSchoolApi.Application.Services;

public interface ITokenGeneratorService
{
    string GenerateJwtAccessToken(Guid userId, UserRole role);
    string GenerateJwtRefreshToken(Guid userId, UserRole role);
}