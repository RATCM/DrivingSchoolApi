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
[Route("debug/student")]
public class StudentDebugController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IPasswordHasher<Student> _studentPasswordHasher;
    private readonly ICompletedCourseRepository _completedCourseRepository;
    private readonly ITheoryLessonRepository _theoryLessonRepository;
    private readonly IDrivingLessonRepository _drivingLessonRepository;

    public StudentDebugController(
        IStudentRepository studentRepository,
        IDrivingSchoolRepository drivingSchoolRepository,
        IPasswordHasher<Student> studentPasswordHasher,
        ICompletedCourseRepository completedCourseRepository,
        ITheoryLessonRepository theoryLessonRepository,
        IDrivingLessonRepository drivingLessonRepository)
    {
        _studentRepository = studentRepository;
        _drivingSchoolRepository = drivingSchoolRepository;
        _studentPasswordHasher = studentPasswordHasher;
        _completedCourseRepository = completedCourseRepository;
        _theoryLessonRepository = theoryLessonRepository;
        _drivingLessonRepository = drivingLessonRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _studentRepository.GetAll();

        return Ok(students.ToList().Select(x => x.ToDto()));
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateStudents(int num = 1, int? seed = null, string? password = null)
    {
        // Random seed if none provided
        seed ??= Guid.NewGuid().GetHashCode();

        var drivingSchools = await _drivingSchoolRepository.GetAll();
        var drivingSchoolIds = drivingSchools.Select(x => x.Id).ToList();
        if (drivingSchoolIds.Count == 0)
            return BadRequest("Cannot add students if there are no driving schools");

        var studentFaker = StudentFaker.Create(seed.Value, drivingSchoolIds, _studentPasswordHasher);
        
        var students = studentFaker.UsePassword(password).Generate(num);

        if (students is null)
            return Problem("Error generating students");
        
        foreach (var student in students)
        {
            var result = await _studentRepository.Create(student);

            if (!result)
                return Problem("Error adding students to database");
        }

        await _studentRepository.Save();
        
        return Ok(students.Select(x => x.ToDto()));
    }
    
    [HttpPost("course/create")]
    public async Task<IActionResult> CreateCompletedCourses(int num, int seed=2)
    {
        var theoryLessons = (await _theoryLessonRepository.GetAll()).ToList();
        var drivingLessons = (await _drivingLessonRepository.GetAll()).ToList();
        if (theoryLessons.Count == 0 && drivingLessons.Count == 0)
            return BadRequest("Cannot add completed courses with no theory lessons or driving lessons");
        
        var completedCourseFaker = CompletedCourseFaker.Create(seed,
            theoryLessons,
            drivingLessons
        );

        var courses = completedCourseFaker.Generate(num);

        if (courses is null)
            return Problem("Error generating completed courses");
        
        foreach (var course in courses)
        {
            var result = await _completedCourseRepository.Create(course);

            if (!result)
                return Problem("Error adding completed courses to database");
        }

        await _completedCourseRepository.Save();
        
        return Ok(courses.Select(x => x.ToDto()));
    }


    
        
    [HttpDelete]
    public async Task<IActionResult> DeleteAllStudents()
    {
        var students = await _studentRepository.GetAll();
        
        foreach (var student in students)
        {
            var deleted = await _studentRepository.Delete(student.Id);

            if (!deleted)
                return Problem("Error deleting students");
        }

        await _studentRepository.Save();
        return NoContent();
    }

}