using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.Filters.Attributes;
using DrivingSchoolApi.Filters.Services;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DrivingSchoolController : ControllerBase
{
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IDrivingSchoolService _drivingSchoolService;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;

    public DrivingSchoolController(
        ILogger<DrivingSchoolController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IDrivingSchoolService drivingSchoolService,
        IStudentService studentService,
        IInstructorService instructorService)
    {
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _drivingSchoolService = drivingSchoolService;
        _studentService = studentService;
        _instructorService = instructorService;
    }
    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistryDto drivingSchool)
    {
        var result = await _drivingSchoolService.CreateDrivingSchool(
            DrivingSchoolName.Create(drivingSchool.Name),
            StreetAddress.Create("N/A", "N/A", "N/A", drivingSchool.Address),
            PhoneNumber.Create(drivingSchool.PhoneNumber),
            WebAddress.Create(drivingSchool.WebAddress),
            []);
        
        return result.IsSuccess
            ? Created($"drivingSchool/{result.Value!.Id}", result.Value.ToDto())
            : this.Problem(result.Error!);
    }
    
    [HttpGet("/{id}")]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetDrivingSchool(Guid id)
    {
        var result = await _drivingSchoolService.GetDrivingSchoolById(DrivingSchoolKey.Create(id));
        
        return result.IsSuccess
            ? Ok(result.Value!.ToDto())
            : this.Problem(result.Error!);
    }
    
    //TODO Add paging
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetAllDrivingSchools()
    {
        var result = await _drivingSchoolService.GetAllDrivingSchools();

        return result.IsSuccess
            ? Ok(result.Value!.Select(x => x.ToDto()))
            : this.Problem(result.Error!);
    }
    
    
    //TODO Add paging
    [HttpGet("{schoolId:guid}/students")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [SameDrivingSchoolFilter("schoolId", TargetEntity.School)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentFromSchool(Guid schoolId)
    {
        var result = await _studentService.GetAllStudentsFromSchool(DrivingSchoolKey.Create(schoolId));
        
        return result.IsSuccess
            ? Ok(result.Value!.Select(s => s.ToDto()))
            : BadRequest("Failed to retrieve students.");
    }
    
    [HttpPost("{schoolId:guid}/student/invite")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<ActionResult<StudentInviteDto>> CreateInvite(Guid schoolId)
    {
        var idClaim = Guid.Parse(HttpContext.GetUserIdClaim()!.Value);

        var instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));

        if (instructor.IsSuccess)
            return this.Problem(instructor.Error!);
        
        var invite = await _drivingSchoolService.CreateStudentInvite(
            DrivingSchoolKey.Create(schoolId), 
            TimeSpan.FromDays(30)); // We just have the invite be available for 30 days for now

        return invite.IsSuccess
            ? Ok(invite.Value!)
            : this.Problem(invite.Error!);
    }
    
    [HttpDelete("{schoolId:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> DeleteDrivingSchool(Guid schoolId)
    {
        var deleted  = await _drivingSchoolService.DeleteDrivingSchool(DrivingSchoolKey.Create(schoolId));
        return deleted.IsSuccess
            ? NoContent()
            : this.Problem(deleted.Error!);
    }
    
    //TODO add paging and filters
    [HttpGet("{schoolId:guid}/theoryLessons")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    //[UserFilter("")]
    public async Task<IActionResult> GetDrivingSchoolTheoryLessons(Guid schoolId)
    {
        var result = await _theoryLessonService.GetAllTheoryLessonsFromSchool(DrivingSchoolKey.Create(schoolId));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    
    //TODO add paging and filters
    [HttpGet("{schoolId:guid}/drivingLessons")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    //[UserFilter("")]
    public async Task<IActionResult> GetDrivingSchoolDrivingLessons(Guid schoolId)
    {
        var result = await _drivingLessonService.GetAllDrivingLessonsFromSchool(DrivingSchoolKey.Create(schoolId));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    
    //TODO add paging and filters
    [HttpGet("{schoolId:guid}/schoolInstructors")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    //[UserFilter("")]
    public async Task<IActionResult> GetDrivingSchoolInstructors(Guid schoolId)
    {
        var result = await _instructorService.GetAllInstructorsFromSchool(DrivingSchoolKey.Create(schoolId));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    //TODO
    //UpdateDrivingSchool
    
    

}
