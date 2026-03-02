using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
using Microsoft.AspNetCore.Authorization;
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
            x.DrivingSchoolName.ToDto(),
            x.StreetAddress.ToDto(),
            x.PhoneNumber.ToDto(),
            x.WebAddress.ToDto(),
            x.PackagePrice.ToDto(),
            null,
            null)));
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistry drivingSchool)
    {
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
    }
}