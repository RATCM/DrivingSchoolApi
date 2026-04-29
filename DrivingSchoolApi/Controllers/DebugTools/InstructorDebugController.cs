using Bogus;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Fakers;
using DrivingSchoolApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers.DebugTools;

[ApiController]
[Route("debug/instructor")]
public class InstructorDebugController : ControllerBase
{
    private readonly IInstructorRepository _instructorRepository;
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IPasswordHasher<Instructor> _instructorPasswordHasher;

    public InstructorDebugController(
        IInstructorRepository instructorRepository,
        IDrivingSchoolRepository drivingSchoolRepository,
        IPasswordHasher<Instructor> instructorPasswordHasher)
    {
        _instructorRepository = instructorRepository;
        _drivingSchoolRepository = drivingSchoolRepository;
        _instructorPasswordHasher = instructorPasswordHasher;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetInstructors()
    {
        var instructors = await _instructorRepository.GetAll();

        return Ok(instructors.ToList().Select(x => x.ToDto()));
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateInstructors(int num = 1, int? seed = null, string? password = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();

        var drivingSchools = await _drivingSchoolRepository.GetAll();
        var drivingSchoolIds = drivingSchools.Select(x => x.Id).ToList();
        if (drivingSchoolIds.Count == 0)
            return BadRequest("Cannot add instructors if there are no driving schools");

        var instructorFaker = InstructorFaker.Create(seed.Value, drivingSchoolIds, _instructorPasswordHasher);
        
        var instructors = instructorFaker.UsePassword(password).Generate(num);

        if (instructors is null)
            return Problem("Error generating instructors");
        
        foreach (var instructor in instructors)
        {
            var result = await _instructorRepository.Create(instructor);

            if (!result)
                return Problem("Error adding instructors to database");
        }

        await _instructorRepository.Save();
        
        return Ok(instructors.Select(x => x.ToDto()));
    }
    
    [HttpPost("scramble")]
    public async Task<IActionResult> ScrambleInstructors(int? seed = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();
        
        var drivingSchools = await _drivingSchoolRepository.GetAll();
        var drivingSchoolIds = drivingSchools.Select(x => x.Id).ToList();
        if (drivingSchoolIds.Count == 0)
            return BadRequest("Cannot scramble students if there are no driving schools");
        
        var faker = InstructorFaker.Create(seed.Value, drivingSchoolIds, _instructorPasswordHasher);
        
        var instructors = (await _instructorRepository.GetAll()).ToList();
        var newInstructors = new List<Instructor>();
        
        foreach (var instructor in instructors)
        {
            var newInstructor = faker.UseId(instructor.Id).Generate();
            if (newInstructor is null)
                return Problem("Error generating instructors");
            
            var updated = await _instructorRepository.Update(newInstructor);
            if (!updated)
                return Problem("Error updating instructors");
            
            newInstructors.Add(newInstructor);
        }
        
        await _instructorRepository.Save();
        
        return Ok(newInstructors.Select(x => x.ToDto()));
    }

            
    [HttpDelete]
    public async Task<IActionResult> DeleteAllInstructors()
    {
        var instructors = await _instructorRepository.GetAll();
        
        foreach (var instructor in instructors)
        {
            var deleted = await _instructorRepository.Delete(instructor.Id);

            if (!deleted)
                return Problem("Error deleting instructors");
        }

        await _instructorRepository.Save();
        return NoContent();
    }
    
}