using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Exceptions.Student;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
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
    
    [HttpGet("/theorylesson")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromStudent()
    {
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(idClaim));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!);
    }

    [HttpGet("/drivinglesson")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    public async Task<IActionResult> GetDrivingLessonsFromStudent()
    {
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _drivingLessonService.GetAllDrivingLessonsFromStudent(StudentKey.Create(idClaim));

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
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        var idClaim = HttpContext.GetUserIdClaim();
        var isAdmin = HttpContext.GetUserRoleClaim().Equals("Admin");

        var deleted = await _studentService.DeleteStudent(studentId, StudentKey.Create(idClaim), isAdmin);

        return deleted.IsSuccess ? 
            NoContent() : 
            this.Problem(deleted.Error!);
    }
    
    

    [HttpGet("/{id:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
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