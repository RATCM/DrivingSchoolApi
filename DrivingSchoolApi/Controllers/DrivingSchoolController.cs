using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
using DrivingSchoolApi.Models;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DrivingSchoolController : ControllerBase
{
    private readonly IDrivingSchoolService _drivingSchoolService;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;

    public DrivingSchoolController(
        ILogger<DrivingSchoolController> logger,
        IDrivingSchoolService drivingSchoolService,
        IStudentService studentService,
        IInstructorService instructorService)
    {
        _drivingSchoolService = drivingSchoolService;
        _studentService = studentService;
        _instructorService = instructorService;
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
    
    
    [HttpGet("{schoolId:guid}/students")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    //[SameDrivingSchoolFilter()]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentFromSchool(Guid schoolId)
    {
        var idClaim = HttpContext.GetUserIdClaim();
        var userId = new Guid(idClaim.Value);
        var roleClaim = HttpContext.User.IsInRole(nameof(UserRole.Instructor));
        
        var instructor = await _instructorService.GetInstructorById(InstructorKey.Create(userId));
        
        if  (!instructor.IsSuccess)
            return this.Problem(instructor.Error!);
        
        var result = await _studentService.GetAllStudentsFromSchool(instructor.Value!.SchoolId);
        
        return result.IsSuccess ? 
            Ok(result.Value!.Select(s => s.ToDto()))
            : BadRequest("Failed to retrieve students.");
    }
}
