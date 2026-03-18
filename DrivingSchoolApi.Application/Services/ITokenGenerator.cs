namespace DrivingSchoolApi.Application.Services;

public interface ITokenGeneratorService
{
    string GenerateJwtAccessToken(Guid userId, string role);
    string GenerateJwtRefreshToken(Guid userId, string role);
}