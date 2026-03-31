using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Filters.Attributes;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
using DrivingSchoolApi.Models;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IStudentService _studentService;

    public InstructorController(
        ILogger<InstructorController> logger,
        IInstructorService instructorService,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService)
    {
        _instructorService = instructorService;
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
    }

    [HttpPost("/login")]
    public async Task<ActionResult> LoginAsInstructor([FromBody] InstructorLoginRequestDto loginRequest)
    {
        var result = await _instructorService.LoginAsInstructor(loginRequest.Email, loginRequest.Password);
        
        return result.IsSuccess ? 
            Ok(new JwtTokenDto{AccessToken = result.Value!.AccessToken, RefreshToken = result.Value.RefreshToken}) :
            this.Problem(result.Error!);
    }
    
    [HttpPost("/register")]
    [Authorize(Policy = AuthPolicies.AdminOnly)] 
    public async Task<ActionResult> RegisterInstructor([FromBody] InstructorRegistryDto registryDto)
    {
        var result = await _instructorService.CreateInstructor(
            registryDto.Name.ToDomain(),
            Email.Create(registryDto.Email),
            registryDto.Password,
            PhoneNumber.Create(registryDto.PhoneNumber),
            DrivingSchoolKey.Create(registryDto.SchoolId));
        
        return result.IsSuccess ?
            Created($"instructor/{result.Value!.Id}", result.Value.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult> GetAllInstructors()
    {
        var result = await _instructorService.GetAllInstructors();
        
        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto())) :
            this.Problem(result.Error!);
    }

    [HttpGet("/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    [UserFilter("instructorId", allowAdmins: true)]
    public async Task<ActionResult> GetInstructorById(Guid instructorId)
    {
        var idClaim = HttpContext.GetUserIdClaim();
        bool isAdmin = HttpContext.User.IsInRole(nameof(UserRole.Admin));
        
        var result = await _instructorService.GetInstructorById(instructorId, isAdmin, InstructorKey.Create(instructorId));
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }
    
    [HttpPut("/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [UserFilter("id")]
    public async Task<ActionResult> UpdateInstructor(Guid instructorId, [FromBody] InstructorUpdateDto updateDto)
    {
        var result = await _instructorService.UpdateInstructor(
            InstructorKey.Create(instructorId),
            DrivingSchoolKey.Create(updateDto.SchoolId),
            updateDto.Name.ToDomain(),
            Email.Create(updateDto.Email),
            PhoneNumber.Create(updateDto.PhoneNumber));
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpPut("/{instructorId:guid}/password")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    [UserFilter("instructorId", allowAdmins: true)]
    public async Task<IActionResult> UpdateInstructorPassword(Guid instructorId, [FromBody] UpdatePasswordDto updateDto)
    {
        var result = await _instructorService.UpdateInstructorPassword(
            InstructorKey.Create(instructorId),
            updateDto.OldPassword,
            updateDto.NewPassword);
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpDelete("/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    [UserFilter("instructorId", allowAdmins: true)]
    public async Task<IActionResult> DeleteInstructor(Guid instructorId)
    {
        var deleted  = await _instructorService.DeleteInstructor(InstructorKey.Create(instructorId));
        return  deleted.IsSuccess ?
            NoContent() :
            this.Problem(deleted.Error!);
    }
    
    [HttpPost("/{instructorId:guid}/theoryLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [UserFilter("instructorId")]
    public async Task<IActionResult> CreateTheoryLesson(Guid instructorId, [FromBody] TheoryLessonRegistryDto registryDto)
    {
        var result = await _theoryLessonService.CreateTheoryLesson(
            InstructorKey.Create(instructorId),
            registryDto.LessonDateTime,
            Money.Create(registryDto.Price.Amount, registryDto.Price.Currency),
            registryDto.StudentIds.Select(StudentKey.Create).ToList());
        
        var created = result.Value!;
        
        return result.IsSuccess ?
            Created($"theoryLesson/{created.Id}", created.ToDto(created.StudentIds)) :
            this.Problem(result.Error!);
    }
    
    [HttpGet("/{instructorId:guid}/theoryLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [UserFilter("instructorId")]
    public async Task<IActionResult> GetTheoryLessonsFromInstructor(Guid instructorId)
    {
        var result = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(instructorId));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto()).ToList()) :
            this.Problem(result.Error!);
    }
    
    [HttpPost("/{instructorId:guid}/drivingLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [UserFilter("instructorId")]
    public async Task<IActionResult> CreateDrivingLesson(Guid instructorId, [FromBody] DrivingLessonRegistryDto registryDto)
    {
        var result = await _drivingLessonService.CreateDrivingLesson(
            DrivingSchoolKey.Create(registryDto.SchoolId),
            registryDto.Route.ToDomain(),
            registryDto.Price.ToDomain(),
            InstructorKey.Create(instructorId),
            StudentKey.Create(registryDto.StudentId));
        
        return result.IsSuccess ?
            Created($"drivingLesson/{result.Value!.Id}", result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }
    
    [HttpGet("{instructorId:guid}/drivingLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [UserFilter("instructorId")]
    public async Task<IActionResult> GetDrivingLessonFromInstructor(Guid instructorId)
    {
        var result = await _drivingLessonService.GetAllDrivingLessonsFromInstructor(InstructorKey.Create(instructorId));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto()).ToList()) :
            this.Problem(result.Error!);
    }
}
