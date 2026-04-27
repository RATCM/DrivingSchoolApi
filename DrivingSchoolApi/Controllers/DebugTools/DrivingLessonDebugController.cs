using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Fakers;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers.DebugTools;

[ApiController]
[Route("debug/drivingLesson")]
public class DrivingLessonDebugController : ControllerBase
{
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITheoryLessonRepository _theoryLessonRepository;
    private readonly IDrivingLessonRepository _drivingLessonRepository;
    
    public DrivingLessonDebugController(
        IDrivingSchoolRepository drivingSchoolRepository,
        IInstructorRepository instructorRepository,
        IStudentRepository studentRepository,
        ITheoryLessonRepository theoryLessonRepository,
        IDrivingLessonRepository drivingLessonRepository)
    {
        _drivingSchoolRepository = drivingSchoolRepository;
        _instructorRepository = instructorRepository;
        _studentRepository = studentRepository;
        _theoryLessonRepository = theoryLessonRepository;
        _drivingLessonRepository = drivingLessonRepository;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAllDrivingLessons()
    {
        var drivingLessons = await _drivingLessonRepository.GetAll();
        
        return Ok(drivingLessons.Select(x => x.ToDto()));
    }
    
    [HttpGet("{drivingLessonId:guid}")]
    public async Task<IActionResult> GetDrivingLesson(Guid drivingLessonId)
    {
        var drivingLesson = await _drivingLessonRepository.Get(DrivingLessonKey.Create(drivingLessonId));

        if (drivingLesson is null)
            return NotFound();

        return Ok(drivingLesson.ToDto());
    }
    
    [HttpGet("{drivingLessonId:guid}/signature/student")]
    public async Task<IActionResult> GetDrivingLessonStudentSignature(Guid drivingLessonId)
    {
        var drivingLesson = await _drivingLessonRepository.Get(DrivingLessonKey.Create(drivingLessonId));

        if (drivingLesson is null)
            return NotFound();

        return File(drivingLesson.StudentSignature.Blob, "image/png");
    }
    
    [HttpGet("{drivingLessonId:guid}/signature/instructor")]
    public async Task<IActionResult> GetDrivingLessonInstructorSignature(Guid drivingLessonId)
    {
        var drivingLesson = await _drivingLessonRepository.Get(DrivingLessonKey.Create(drivingLessonId));

        if (drivingLesson is null)
            return NotFound();

        return File(drivingLesson.InstructorSignature.Blob, "image/png");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDrivingLessons(int num, int? seed = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();

        var drivingSchools = await _drivingSchoolRepository.GetAll();
        var instructors = await _instructorRepository.GetAll();
        var students = await _studentRepository.GetAll();
        var drivingLessonFaker = 
            DrivingLessonFaker.Create(
                seed.Value, 
                drivingSchools, 
                instructors, 
                students);

        if (!drivingLessonFaker.IsSuccess)
            return this.Problem(drivingLessonFaker.Error!);

        var drivingLessons = drivingLessonFaker.Value!.Generate(num);

        if (drivingLessons is null)
            return Problem("Error generating driving lessons");

        foreach (var drivingLesson in drivingLessons)
        {
            var result = await _drivingLessonRepository.Create(drivingLesson);
            
            if (!result)
                return Problem("Error adding driving lessons to database");
        }

        await _theoryLessonRepository.Save();

        return Ok(drivingLessons.Select(x => x.ToDto()));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllDrivingLessons()
    {
        var drivingLessons = await _drivingLessonRepository.GetAll();
        
        
        foreach (var drivingLesson in drivingLessons)
        {
            var deleted = await _drivingLessonRepository.Delete(drivingLesson.Id);

            if (!deleted)
                return Problem("Error deleting driving lessons");
        }

        await _drivingLessonRepository.Save();
        return NoContent();
    }


}