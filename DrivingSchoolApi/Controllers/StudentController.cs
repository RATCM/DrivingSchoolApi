using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
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
    [Authorize]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDtoRegistry student)
    {
        /*
public sealed record StudentDtoRegistry(
    Guid SchoolId,
    NameDto StudentName,
    EmailDto EmailAddress,
    PhoneNumberDto PhoneNumber,
    string Password);
        */
        var created = await _studentService.CreateStudent(
            Name.Create(student.StudentName.FirstName, student.StudentName.LastName),
        )
        /*
        var created = await _drivingSchoolService.CreateDrivingSchool(
            DrivingSchoolName.Create(drivingSchool.Name),
            StreetAddress.Create("N/A", "N/A", "N/A", drivingSchool.Address),
            PhoneNumber.Create(drivingSchool.PhoneNumber),
            WebAddress.Create(drivingSchool.WebAddress),
            Money.Create(
                decimal.Parse(drivingSchool.PackagePrice.Split(" ")[0]),
                drivingSchool.PackagePrice.Split(" ")[1]));;

        return Created($"drivingschool/{created.Id}", new DrivingSchoolDto(
            created.Id.Value,
            created.DrivingSchoolName.ToDto(),
            created.StreetAddress.ToDto(),
            created.PhoneNumber.ToDto(),
            created.WebAddress.ToDto(),
            created.PackagePrice.ToDto(),
            null,
            null));
         */
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentById(StudentKey id)
    {
        
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentFromSchool(DrivingSchoolKey schoolId)
    {
        
    }
}