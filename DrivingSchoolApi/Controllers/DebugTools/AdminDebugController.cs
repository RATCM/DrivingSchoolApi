using Bogus;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Fakers;
using DrivingSchoolApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers.DebugTools;

[ApiController]
[Route("debug/admin")]
public class AdminDebugController : ControllerBase
{
    private readonly IAdminRepository _adminRepository;
    private readonly IPasswordHasher<Admin> _adminPasswordHasher;
    public AdminDebugController(
        IAdminRepository adminRepository,
        IPasswordHasher<Admin> adminPasswordHasher)
    {
        _adminRepository = adminRepository;
        _adminPasswordHasher = adminPasswordHasher;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAdmins(int num = 1, int seed = 1, string? password = null)
    {
        var adminFaker = AdminFaker.Create(seed, _adminPasswordHasher);

        var admins = adminFaker.UsePassword(password).Generate(num);

        if (admins is null)
            return Problem("Error generating admins");
        
        foreach (var admin in admins)
        {
            var result = await _adminRepository.Create(admin);

            if (!result)
                return Problem("Error adding admins to database");
        }

        await _adminRepository.Save();
        
        return Ok(admins.Select(x => x.ToDto()));
    }

    [HttpGet]
    public async Task<IActionResult> GetAdmins()
    {
        var admins = await _adminRepository.GetAll();

        return Ok(admins.Select(x => x.ToDto()));
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAllAdmins()
    {
        var admins = await _adminRepository.GetAll();

        foreach (var admin in admins)
            await _adminRepository.Delete(admin.Id);

        await _adminRepository.Save();
        return NoContent();
    }
}