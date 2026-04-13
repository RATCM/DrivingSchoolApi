using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.Admin;
using DrivingSchoolApi.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApi.Utils;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IAdminService _adminService;
    public AdminController(
        ILogger<AdminController> logger, 
        IAdminService adminService)
    {
        _logger = logger;
        _adminService = adminService;
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> CreateAdmin(AdminRegistryDto registry)
    {
        var result = await _adminService.CreateAdmin(Email.Create(registry.Email), registry.Password);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error creating admin: {Message}", result.Error!.Message);
            this.Problem(result.Error!);
        }

        _logger.LogInformation("Admin created with id: {Id}", result.Value!.Id.Value);
        return Ok(result.Value!);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsAdmin(LoginDto login)
    {
        var result = await _adminService.LoginAsAdmin(Email.Create(login.Email), login.Password);

        return result.IsSuccess
            ? Ok(new JwtTokenDto 
            {
                AccessToken = result.Value!.accessToken,
                RefreshToken = result.Value!.refreshToken
            })
            : this.Problem(result.Error!);
    }
}