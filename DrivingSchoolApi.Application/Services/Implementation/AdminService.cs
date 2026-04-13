using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Exceptions.Admin;
using DrivingSchoolApi.Application.Exceptions.Common;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

public class AdminService : IAdminService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IPasswordHasher<Admin> _passwordHasher;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IAdminRepository _adminRepository;
    public AdminService(
        IGuidGeneratorService guidGeneratorService,
        IPasswordHasher<Admin> passwordHasher,
        ITokenGeneratorService tokenGeneratorService,
        IAdminRepository adminRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _passwordHasher = passwordHasher;
        _tokenGeneratorService = tokenGeneratorService;
        _adminRepository = adminRepository;
    }
    
    public async Task<Result<Admin>> CreateAdmin(Email email, string password)
    {
        var admin = Admin.Create(
            AdminKey.Create(_guidGeneratorService.NewGuid()),
            email,
            _passwordHasher.HashPassword(password));

        var result = await _adminRepository.Create(admin);

        if(!result) 
            return new Exception("Unable to create admin");

        await _adminRepository.Save();
        return admin;
    }

    public async Task<Result<(string accessToken, string refreshToken)>> LoginAsAdmin(Email email, string password)
    {
        var admin = await _adminRepository.GetByEmail(email);

        if (admin is null)
            return new AdminNotFoundException("Admin could not be found");

        var passwordValid = _passwordHasher.VerifyHashedPassword(password, admin.HashedPassword);

        var accessToken = _tokenGeneratorService.GenerateJwtAccessToken(admin.Id.Value, UserRole.Admin);
        var refreshToken = _tokenGeneratorService.GenerateJwtRefreshToken(admin.Id.Value, UserRole.Admin);
        return passwordValid
            ? (accessToken, refreshToken)
            : new InvalidLoginRequestException();
    }
    
    public async Task<Result<Admin>> GetAdminById(AdminKey id)
    {
        var admin = await _adminRepository.Get(id);

        if(admin is null)
            return new AdminNotFoundException("Admin could not be found");
        
        return admin;
    }

    public async Task<Result<List<Admin>>> GetAllAdmins()
    {
        var admins = await _adminRepository.GetAll();

        return admins.ToList();
    }

    public async Task<Result<Admin>> UpdateAdmin(AdminKey id, Email? newEmail = null, string? newPassword = null)
    {
        var admin = await _adminRepository.Get(id);
        if(admin is null)
            return new AdminNotFoundException("Admin could not be found");
        
        var newAdmin = Admin.Create(
            id,
            newEmail ?? admin.EmailAddress,
            newPassword is not null 
                ? _passwordHasher.HashPassword(newPassword) 
                : admin.HashedPassword);
        
        var updated = await _adminRepository.Update(newAdmin);
        
        if(!updated)
            return new Exception("Admin could not be updated");

        await _adminRepository.Save();
        return newAdmin;
    }

    public async Task<Result> DeleteAdmin(AdminKey id)
    {
        var deleted = await _adminRepository.Delete(id);
        
        if(!deleted)
            return new AdminNotFoundException("Admin could not be found");
        
        await _adminRepository.Save();
        return Result.Success();
    }
}