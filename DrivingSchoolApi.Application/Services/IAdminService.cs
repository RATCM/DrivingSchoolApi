using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IAdminService
{
    Task<Result<Admin>> CreateAdmin(Email email, string password);
    Task<Result<(string accessToken, string refreshToken)>> LoginAsAdmin(Email email, string password);
    Task<Result<Admin>> GetAdminById(AdminKey id);
    Task<Result<List<Admin>>> GetAllAdmins();
    Task<Result<Admin>> UpdateAdmin(AdminKey id, Email? newEmail = null, string? newPassword = null);
    Task<Result> DeleteAdmin(AdminKey id);
}