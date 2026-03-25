using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
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
        
        var created = result.Value!;
        
        return result.IsSuccess ?
            Created($"instructor/{created.Id}", created.ToDto()) :
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
    public async Task<ActionResult> GetInstructorById(Guid instructorId)
    {
        // Read Jwt Token to ascertain  ID
        var idClaim = HttpContext.GetUserIdClaim();
        bool isAdmin = HttpContext.GetUserRoleClaim().Equals("admin");
        
        var result = await _instructorService.GetInstructorById(instructorId, isAdmin, InstructorKey.Create(instructorId));
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }
    
    [HttpPut("/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<ActionResult> UpdateInstructor(Guid instructorId, [FromBody] InstructorUpdateDto updateDto)
    {
        // Read Jwt Token to ascertain Instructor ID
        var idClaim = HttpContext.GetUserIdClaim();
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only update their own attributes.");
        
        var result = await _instructorService.UpdateInstructor(
            InstructorKey.Create(idClaim),
            DrivingSchoolKey.Create(updateDto.SchoolId),
            updateDto.Name.ToDomain(),
            Email.Create(updateDto.Email),
            PhoneNumber.Create(updateDto.PhoneNumber));
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpPut("/{instructorId:guid}/password")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> UpdateInstructorPassword(Guid instructorId, [FromBody] UpdatePasswordDto updateDto)
    {
        // Read Jwt Token to ascertain Instructor ID
        var idClaim = HttpContext.GetUserIdClaim();
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only update their own password.");
        
        var result = await _instructorService.UpdateInstructorPassword(
            InstructorKey.Create(idClaim),
            updateDto.OldPassword,
            updateDto.NewPassword);
        
        return result.IsSuccess ?
            Ok(result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpDelete("/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    public async Task<IActionResult> DeleteInstructor(Guid instructorId)
    {
        // Read Jwt Token to ascertain Instructor ID
        var idClaim = HttpContext.GetUserIdClaim();
        
        // TODO enforce validation
        // Instructor can only delete their own data
        // Admin can delete all
        // If possible do it in Application.Services
        
        var deleted  = await _instructorService.DeleteInstructor(
            InstructorKey.Create(idClaim));
        
        return  deleted.IsSuccess ?
            NoContent() :
            this.Problem(deleted.Error!);
    }
    
    [HttpPost("/{instructorId:guid}/theoryLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateTheoryLesson(Guid instructorId, [FromBody] TheoryLessonRegistryDto registryDto)
    {
        // Read Jwt Token to ascertain Instructor ID
        var idClaim = HttpContext.GetUserIdClaim();
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only create theory lessons containing themself.");
        
        var result = await _theoryLessonService.CreateTheoryLesson(
            InstructorKey.Create(idClaim),
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
    public async Task<IActionResult> GetTheoryLessonsFromInstructor(Guid instructorId)
    {
        // Check Jwt Token to ascertain Instructor ID
        var idClaim = HttpContext.GetUserIdClaim();
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only see their own theory lessons.");
        
        var result = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(idClaim));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto()).ToList()) :
            this.Problem(result.Error!);
    }
    
    [HttpPost("/{instructorId:guid}/drivingLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateDrivingLesson(Guid instructorId, [FromBody] DrivingLessonRegistryDto registryDto)
    {
        // Check Jwt Token to ascertain Instructor ID
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only create driving lessons containing themself.");
        
        var result = await _drivingLessonService.CreateDrivingLesson(
            DrivingSchoolKey.Create(registryDto.SchoolId),
            registryDto.Route.ToDomain(),
            registryDto.Price.ToDomain(),
            InstructorKey.Create(idClaim),
            StudentKey.Create(registryDto.StudentId));
        
        return result.IsSuccess ?
            Created($"drivingLesson/{result.Value!.Id}", result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }
    
    [HttpGet("{instructorId:guid}/drivingLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> GetDrivingLessonFromInstructor(Guid instructorId)
    {
        // Check Jwt Token to ascertain Instructor ID
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        
        if (instructorId != idClaim)
            return Forbid("Instructors can only see driving lessons containing themself.");
        
        var result = await _drivingLessonService.GetAllDrivingLessonsFromInstructor(InstructorKey.Create(idClaim));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto()).ToList()) :
            this.Problem(result.Error!);
    }
}
