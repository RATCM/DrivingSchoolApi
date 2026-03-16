using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(
        ILogger<StudentController> logger,
        IStudentService studentService)
    {
        _studentService = studentService;
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