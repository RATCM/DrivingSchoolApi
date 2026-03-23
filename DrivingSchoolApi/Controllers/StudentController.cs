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
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromStudent()
    {
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(idClaim));
        if (!result.IsSuccess) { return BadRequest("Failed to retrieve theory lessons."); }
        var theoryLessons = result.Value!;
        
        return Ok(theoryLessons.Select(x => new TheoryLessonDto(
            x.Id.Value,
            x.SchoolId.Value,
            x.InstructorId.Value,
            x.LessonDateTime,
            x.Price.ToDto(),
            x.StudentIds.Select(studentKey => studentKey.Value).ToList()
        )));
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
    public async Task<Result<Student>> GetStudentById(StudentKey id)
    {
        var student = await _studentRepository.Get(id);
        if (student is null)
            return new StudentNotFoundException("Student not found.");
        return student;
    }
}