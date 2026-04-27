using System.ComponentModel.DataAnnotations;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Fakers;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers.DebugTools;

[ApiController]
[Route("debug/theoryLesson")]
public class TheoryLessonDebugController : ControllerBase
{
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITheoryLessonRepository _theoryLessonRepository;

    public TheoryLessonDebugController(
        IDrivingSchoolRepository drivingSchoolRepository,
        IInstructorRepository instructorRepository,
        IStudentRepository studentRepository,
        ITheoryLessonRepository theoryLessonRepository)
    {
        _drivingSchoolRepository = drivingSchoolRepository;
        _instructorRepository = instructorRepository;
        _studentRepository = studentRepository;
        _theoryLessonRepository = theoryLessonRepository;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllTheoryLessons()
    {
        var theoryLessons = await _theoryLessonRepository.GetAll();
        
        return Ok(theoryLessons.Select(x => x.ToDto()));
    }
    
    [HttpGet("{theoryLessonId:guid}")]
    public async Task<IActionResult> GetTheoryLesson(Guid theoryLessonId)
    {
        var theoryLesson = await _theoryLessonRepository.Get(TheoryLessonKey.Create(theoryLessonId));

        if (theoryLesson is null)
            return NotFound();

        return Ok(theoryLesson.ToDto());
    }
    
    [HttpGet("{theoryLessonId:guid}/signature/student")]
    public async Task<IActionResult> GetTheoryLessonStudentSignature(Guid theoryLessonId)
    {
        var theoryLesson = await _theoryLessonRepository.Get(TheoryLessonKey.Create(theoryLessonId));

        if (theoryLesson is null)
            return NotFound();

        return File(theoryLesson.StudentSignature.Blob, "image/png");
    }
    
    [HttpGet("{theoryLessonId:guid}/signature/instructor")]
    public async Task<IActionResult> GetTheoryLessonInstructorSignature(Guid theoryLessonId)
    {
        var theoryLesson = await _theoryLessonRepository.Get(TheoryLessonKey.Create(theoryLessonId));

        if (theoryLesson is null)
            return NotFound();

        return File(theoryLesson.InstructorSignature.Blob, "image/png");
    }

    
    [HttpPost("create")]
    public async Task<IActionResult> CreateTheoryLessons([Required] int num, int? seed = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();

        var drivingSchools = await _drivingSchoolRepository.GetAll();
        var instructors = await _instructorRepository.GetAll();
        var students = await _studentRepository.GetAll();
        var theoryLessonFaker = 
            TheoryLessonFaker.Create(
                seed.Value, 
                drivingSchools, 
                instructors, 
                students);

        if (!theoryLessonFaker.IsSuccess)
            return this.Problem(theoryLessonFaker.Error!);

        var theoryLessons = theoryLessonFaker.Value!.Generate(num);

        if (theoryLessons is null)
            return Problem("Error generating theory lessons");

        foreach (var theoryLesson in theoryLessons)
        {
            var result = await _theoryLessonRepository.Create(theoryLesson);
            
            if (!result)
                return Problem("Error adding theory lessons to database");
        }

        await _theoryLessonRepository.Save();

        return Ok(theoryLessons.Select(x => x.ToDto()));
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAllTheoryLessons()
    {
        var theoryLessons = await _theoryLessonRepository.GetAll();
        
        foreach (var theoryLesson in theoryLessons)
        {
            var deleted = await _theoryLessonRepository.Delete(theoryLesson.Id);

            if (!deleted)
                return Problem("Error deleting theory lessons");
        }

        await _theoryLessonRepository.Save();
        return NoContent();
    }

}