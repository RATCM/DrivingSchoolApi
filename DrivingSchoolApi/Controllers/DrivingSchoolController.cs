using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
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
public class DrivingSchoolController : ControllerBase
{
    private readonly IDrivingSchoolService _drivingSchoolService;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;

    public DrivingSchoolController(
        ILogger<DrivingSchoolController> logger,
        IDrivingSchoolService drivingSchoolService,
        IStudentService studentService,
        IStudentRepository studentRepository,
        IInstructorService instructorService)
    {
        _drivingSchoolService = drivingSchoolService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetAllDrivingSchools()
    {
        var drivingSchools = await _drivingSchoolService.GetAllDrivingSchools();

        if (!drivingSchools.IsSuccess) 
            return Problem(drivingSchools.Error!.Message, "", 500);
        return Ok(drivingSchools.Value!.Select(x => new DrivingSchoolDto(
            x.Id.Value,
            x.DrivingSchoolName.ToDto(),
            x.StreetAddress.ToDto(),
            x.PhoneNumber.ToDto(),
            x.WebAddress.ToDto(),
            x.Packages.Select(y => y.ToDto()).ToList(),
            null,
            null)));
    }
    
    //[HttpGet("{id}")]
    //public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetDrivingSchool(Guid id)
    //{
    //    var drivingSchools = await _drivingSchoolService.GetDrivingSchool(id);
    //    return Ok(drivingSchools.Select(x => new DrivingSchoolDto(
    //        x.Id.Value,
    //        x.DrivingSchoolName.ToDto(),
    //        x.StreetAddress.ToDto(),
    //        x.PhoneNumber.ToDto(),
    //        x.WebAddress.ToDto(),
    //        x.PackagePrice.ToDto(),
    //        null,
    //        null)));
    //}
    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistryDto drivingSchool)
    {
        var createdResult = await _drivingSchoolService.CreateDrivingSchool(
            DrivingSchoolName.Create(drivingSchool.Name),
            StreetAddress.Create("N/A", "N/A", "N/A", drivingSchool.Address),
            PhoneNumber.Create(drivingSchool.PhoneNumber),
            WebAddress.Create(drivingSchool.WebAddress),
            []);
        
        if (!createdResult.IsSuccess) 
            return Problem(createdResult.Error!.Message, "", 500);
        var created = createdResult.Value!;
        
        return Created($"drivingschool/{created.Id}", new DrivingSchoolDto(
            created.Id.Value,
            created.DrivingSchoolName.ToDto(),
            created.StreetAddress.ToDto(),
            created.PhoneNumber.ToDto(),
            created.WebAddress.ToDto(),
            created.Packages.Select(x => x.ToDto()).ToList()));
    }
    [HttpGet]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentFromSchool()
    {
        var idClaim = HttpContext.GetUserIdClaim();
        var instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
        var result = await _studentService.GetAllStudentsFromSchool(instructor.Value.SchoolId);
        if (!result.IsSuccess)
            return BadRequest("Failed to retrieve students.");
        var students = result.Value!;
        return Ok(students.Select(s => s.ToDto()));
    }
}