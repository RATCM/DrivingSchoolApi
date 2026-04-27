using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.Instructor;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<InstructorController> _logger;
    private readonly IAdminService _adminService;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    
    public AuthController(
        ILogger<InstructorController> logger,
        IAdminService adminService,
        IStudentService studentService,
        IInstructorService instructorService,
        ITokenGeneratorService tokenGeneratorService)
    {
        _logger = logger;
        _adminService = adminService;
        _studentService = studentService;
        _instructorService = instructorService;
        _tokenGeneratorService = tokenGeneratorService;
    }
    
    [HttpPost("refresh")]
    [Authorize(AuthSchemes.Refresh)]
    public async Task<ActionResult> RefreshTokenAdmin()
    {
        var userId = Guid.Parse(HttpContext.GetUserIdClaim()!.Value);
        var userRole = HttpContext.GetUserRoleClaim()!.Value;
        var newAccessToken = _tokenGeneratorService.GenerateJwtAccessToken(userId, userRole);
        
        // could do validation of the newAccessToken

        return Ok(
            new JwtTokenDto
                { AccessToken = newAccessToken, RefreshToken = null }
        );
    }
    
    [HttpPost("login/student")]
    public async Task<ActionResult> LoginAsStudent([FromBody] StudentLoginRequestDto loginRequest)
    {
        var result = await _studentService.LoginAsStudent(loginRequest.Email, loginRequest.Password);
        
        return result.IsSuccess
            ? Ok(new JwtTokenDto{AccessToken = result.Value!.AccessToken, RefreshToken = result.Value.RefreshToken})
            : this.Problem(result.Error!);
    }
    
    [HttpPost("login/instructor")]
    public async Task<ActionResult> LoginAsInstructor([FromBody] InstructorLoginRequestDto loginRequest)
    {
        var result = await _instructorService.LoginAsInstructor(loginRequest.Email, loginRequest.Password);
        
        return result.IsSuccess
            ? Ok(new JwtTokenDto{AccessToken = result.Value!.AccessToken, RefreshToken = result.Value.RefreshToken})
            : this.Problem(result.Error!);
    }
    
    [HttpPost("login/admin")]
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
