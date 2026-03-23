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
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentService _studentService;

    public StudentController(
        ILogger<StudentController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService,
        IStudentRepository studentRepository)
    {
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
        _studentRepository = studentRepository;
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
        //if (!result.IsSuccess) { return BadRequest("Failed to retrieve theory lessons."); }
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem((result.Error!));
    }
    
    
    [HttpPost]
    //TODO Add invite ID
    public async Task<IActionResult> CreateStudent([FromBody] StudentRegistryDto student)
    {
        var createdResult = await _studentService.CreateStudent(
            Name.Create(student.StudentName.FirstName, student.StudentName.LastName),
            Email.Create(student.EmailAddress),
            student.Password,
            PhoneNumber.Create(student.PhoneNumber),
            DrivingSchoolKey.Create(student.SchoolId));
        
        if (!createdResult.IsSuccess) 
            return Problem("Error creating student", "", 500);
        var created = createdResult.Value!;


        return Created($"student/{created.Id}", new StudentDto(
            created.Id.Value,
            created.SchoolId.Value,
            created.StudentName.ToDto(),
            created.EmailAddress.ToDto(),
            created.PhoneNumber.ToDto()));
    }

    [HttpGet]
    public async Task<Result<StudentDto>> GetStudentById(StudentKey id)
    {
        var student = await _studentRepository.Get(id);
        if (student is null)
            return new StudentNotFoundException();
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(id);
        var drivingLessons = await _drivingLessonService.GetAllDrivingLessonsFromStudent(id);
        
        return student.ToDto(theoryLessons: theoryLessons.Value, drivingLessons: drivingLessons.Value);
    }
}