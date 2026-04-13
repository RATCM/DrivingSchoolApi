using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Filters.Attributes;
using DrivingSchoolApi.Filters.Services;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IStudentService _studentService;

    public StudentController(
        ILogger<StudentController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService)
    {
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
    }
    
    
    //TODO login
    [HttpPost("/login")]
    public async Task<ActionResult> LoginAsStudent([FromBody] StudentLoginRequestDto loginRequest)
    {
        var result = await _studentService.LoginAsStudent(loginRequest.Email, loginRequest.Password);
        
        return result.IsSuccess
            ? Ok(new JwtTokenDto{AccessToken = result.Value!.AccessToken, RefreshToken = result.Value.RefreshToken})
            : this.Problem(result.Error!);
    }
    
    
    //TODO register (should be implemented studentInvite branch)
    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents(int page = 1)
    {
        var result = await _studentService.GetAllStudents();
        const int PAGE_SIZE = 30;
        return result.IsSuccess
            ? Ok(result.Value!.Skip(PAGE_SIZE*(page-1)).Take(PAGE_SIZE).Select(x => x.ToDto()))
            : this.Problem(result.Error!);
    }
    
    
    [HttpGet("/{studentId:guid}/theorylessons")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    [UserFilter("studentId")]
    public async Task<IActionResult> GetTheoryLessonsFromStudent(Guid studentId)
    {
        var result = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(studentId));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }

    
    [HttpGet("{studentId:guid}/drivinglesson/")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    [UserFilter("studentId")]
    public async Task<IActionResult> GetDrivingLessonsFromStudent(Guid studentId)
    {
        var result = await _drivingLessonService.GetAllDrivingLessonsFromStudent(StudentKey.Create(studentId));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    
    
    [HttpPost]
    //TODO Add invite ID
    public async Task<IActionResult> CreateStudent([FromBody] StudentRegistryDto student)
    {
        var result = await _studentService.CreateStudent(
            Name.Create(student.StudentName.FirstName, student.StudentName.LastName),
            Email.Create(student.EmailAddress),
            student.Password,
            PhoneNumber.Create(student.PhoneNumber),
            DrivingSchoolKey.Create(student.SchoolId));
        
        var created = result.Value!;


        return result.IsSuccess ?
            Created($"student/{created.Id}", result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    
    [HttpDelete("/{studentId:Guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrStudent)]
    [UserFilter("studentId", letAdminsBypass: true)]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {

        var deleted = await _studentService.DeleteStudent(StudentKey.Create(studentId));

        return deleted.IsSuccess ? 
            NoContent() : 
            this.Problem(deleted.Error!);
    }
    
    
    [HttpGet("/{id:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    [SameDrivingSchoolFilter("id", TargetEntity.Student,true)]
    public async Task<ActionResult<StudentDto>> GetStudentById(Guid id)
    {
        var student = await _studentService.GetStudentById(StudentKey.Create(id));
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(id));
        var drivingLessons = await _drivingLessonService.GetAllDrivingLessonsFromStudent(StudentKey.Create(id));

        return student.IsSuccess ?
            Ok(student.Value!.ToDto(theoryLessons: theoryLessons.Value, drivingLessons: drivingLessons.Value)) :
            this.Problem(student.Error!);
    }
}
