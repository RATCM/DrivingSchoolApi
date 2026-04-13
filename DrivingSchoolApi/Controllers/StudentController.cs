using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.Student;
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
    private readonly IStudentInviteService _studentInviteService;

    public StudentController(
        ILogger<StudentController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService,
        IStudentInviteService studentInviteService)
    {
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
        _studentInviteService = studentInviteService;
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents(int page = 1)
    {
        var result = await _studentService.GetAllStudents();
        const int PAGE_SIZE = 30;
        return result.IsSuccess ? 
            Ok(result.Value!.Skip(PAGE_SIZE*(page-1)).Take(PAGE_SIZE).Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    
    [HttpGet]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromStudent()
    {
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(idClaim));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRegistryDto student)
    {
        var studentInviteResult = await _studentInviteService.RedeemStudentInvite(
            StudentInviteKey.Create(student.InviteId));

        if (!studentInviteResult.IsSuccess)
            return this.Problem(studentInviteResult.Error!);
        
        var result = await _studentService.CreateStudent(
            Name.Create(student.StudentName.FirstName, student.StudentName.LastName),
            Email.Create(student.EmailAddress),
            student.Password,
            PhoneNumber.Create(student.PhoneNumber),
            studentInviteResult.Value!.Id);
        
        var created = result.Value!;

        return result.IsSuccess ?
            Created($"student/{created.Id}", result.Value!.ToDto()) :
            this.Problem(result.Error!);
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    public async Task<ActionResult<StudentDto>> GetStudentById(StudentKey id)
    {
        var student = await _studentService.GetStudentById(id);
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(id);
        var drivingLessons = await _drivingLessonService.GetAllDrivingLessonsFromStudent(id);

        return student.IsSuccess ?
            Ok(student.Value!.ToDto(theoryLessons: theoryLessons.Value, drivingLessons: drivingLessons.Value)) :
            this.Problem(student.Error!);
    }
}