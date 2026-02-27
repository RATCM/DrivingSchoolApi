using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DrivingSchoolController : ControllerBase
{
    private readonly IDrivingSchoolService _drivingSchoolService;

    public DrivingSchoolController(
        ILogger<DrivingSchoolController> logger,
        IDrivingSchoolService drivingSchoolService)
    {
        _drivingSchoolService = drivingSchoolService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetAllDrivingSchools()
    {
        var drivingSchools = await _drivingSchoolService.GetAllDrivingSchools();
        return Ok(drivingSchools.Select(x => new DrivingSchoolDto(
            x.Id.Value,
            x.DrivingSchoolName.Name,
            x.SchoolAddress.ToString(),
            x.PhoneNumber.Number,
            x.WebAddress.Url,
            "N/A")));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistry drivingSchool)
    {
        var created = await _drivingSchoolService.CreateDrivingSchool(
            DrivingSchoolName.Create(drivingSchool.Name),
            Address.Create("N/A", "N/A", "N/A", drivingSchool.Address),
            PhoneNumber.Create(drivingSchool.PhoneNumber),
            WebAddress.Create(drivingSchool.WebAddress));
        
        return Created($"drivingschool/{created.Id}", new DrivingSchoolDto(
            created.Id.Value,
            created.DrivingSchoolName.Name,
            created.SchoolAddress.ToString(),
            created.PhoneNumber.Number,
            created.WebAddress.Url,
            "N/A"));
    }
}