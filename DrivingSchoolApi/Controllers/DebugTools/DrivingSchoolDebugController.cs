using System.ComponentModel.DataAnnotations;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Fakers;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers.DebugTools;

[ApiController]
[Route("debug/drivingSchool")]
public class DrivingSchoolDebugController : ControllerBase
{
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    
    public DrivingSchoolDebugController(
        IDrivingSchoolRepository drivingSchoolRepository)
    {
        _drivingSchoolRepository = drivingSchoolRepository;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDrivingSchools([Required] int num, int? seed = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();

        var drivingSchoolFaker = DrivingSchoolFaker.Create(seed.Value);

        var drivingSchools = drivingSchoolFaker.Generate(num);
        
        if (drivingSchools is null)
            return Problem("Error generating driving schools");
        
        foreach (var drivingSchool in drivingSchools)
        {
            var result = await _drivingSchoolRepository.Create(drivingSchool);

            if (!result)
                return Problem("Error adding driving schools to database");
        }

        await _drivingSchoolRepository.Save();
        
        return Ok(drivingSchools.Select(x => x.ToDto()));
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAllDrivingSchools()
    {
        var drivingSchools = await _drivingSchoolRepository.GetAll();

        foreach (var drivingSchool in drivingSchools)
        {
            var deleted = await _drivingSchoolRepository.Delete(drivingSchool.Id);

            if (!deleted)
                return Problem("Error deleting driving schools");
        }

        await _drivingSchoolRepository.Save();
        return NoContent();
    }

}