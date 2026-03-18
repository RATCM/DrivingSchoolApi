using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IPasswordHasher<T> where T : class
{
    PasswordHash HashPassword(string password);
    bool VerifyHashedPassword(string password, PasswordHash hash);
}