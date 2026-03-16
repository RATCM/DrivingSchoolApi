using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Exceptions.Instructor;
using DrivingSchoolApi.Application.Exceptions.Student;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class InstructorController : ControllerBase
{
    
    private readonly IInstructorService _instructorService;
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IStudentService _studentService;

    public InstructorController(
        ILogger<InstructorController> logger,
        IInstructorService instructorService,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService)
    {
        _instructorService = instructorService;
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromBody] InstructorLoginRequestDto loginRequest)
    {
        //TODO
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<InstructorDto>>> GetAllInstructors()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateTheoryLesson([FromBody] TheoryLessonRegistry theoryLessonRegistry)
    {
        
        //TODO Authorization: Instructor should only be able to create lessons for themselves and their own school
        
        var created = await _theoryLessonService.CreateTheoryLesson(
            DrivingSchoolKey.Create(theoryLessonRegistry.SchoolId),
            theoryLessonRegistry.LessonDateTime,
            Money.Create(theoryLessonRegistry.Price.Amount, theoryLessonRegistry.Price.Currency),
            InstructorKey.Create(theoryLessonRegistry.InstructorId),
            theoryLessonRegistry.StudentIds.Select(StudentKey.Create).ToList());
        
        return Created($"theoryLesson/{created.Id}", new TheoryLessonDto(
            created.Id.Value,
            created.SchoolId.Value,
            created.InstructorId.Value,
            created.LessonDateTime,
            created.Price.ToDto(),
            created.StudentIds.Select(studentKey => studentKey.Value).ToList()
            ));
    }
    
    [HttpGet("/theoryLessons/{instructorId:guid}")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromInstructor(Guid instructorId)
    {
        
        //TODO Authorization: Instructor should only be able to access their own lessons
        
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(instructorId));
        return Ok(theoryLessons.Select(x => new TheoryLessonDto(
            x.Id.Value,
            x.SchoolId.Value,
            x.InstructorId.Value,
            x.LessonDateTime,
            x.Price.ToDto(),
            x.StudentIds.Select(studentKey => studentKey.Value).ToList()
        )));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTheoryLessonsFromStudent(Guid studentId)
    {
        //TODO Authorization: Students should only be able to access their own lessons
        
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(studentId));
        return Ok(theoryLessons.Select(x => new TheoryLessonDto(
            x.Id.Value,
            x.SchoolId.Value,
            x.InstructorId.Value,
            x.LessonDateTime,
            x.Price.ToDto(),
            x.StudentIds.Select(studentKey => studentKey.Value).ToList()
        )));
     }

    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateDrivingLesson([FromBody] DrivingLessonRegistry registry)
    {
        // Check Jwt Token to ascertain Instructor ID
        // Enforces that Instructor can only create lessons for themselves
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        Instructor instructor;
        Student student;
        
        // Check that instructor & student exists in DB
        try
        {
            instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
            student = await _studentService.GetStudentById(StudentKey.Create(registry.StudentId));
        }
        catch (InstructorNotFoundException e)
        {
            return NotFound("Instructor not found. " + e.Message);
        }
        catch (StudentNotFoundException e)
        {
            return NotFound("Student not found. " + e.Message);
        }

        // Check that student is from the same school as instructor
        if (student.SchoolId.Value != instructor.SchoolId.Value)
        {
            return BadRequest("Student is not assigned to the same school as the instructor.");
        }
        
        //TODO check that information is correct (e.g. price is not negative, route is valid etc.)
        
        var created = await _drivingLessonService.CreateDrivingLesson(
            instructor.SchoolId,
            registry.Route.ToDomain(),
            registry.Price.ToDomain(),
            instructor.Id,
            StudentKey.Create(registry.StudentId));
        
        return Created($"drivingLesson/{created.Id}", new DrivingLessonDto(
            created.Id.Value,
            created.SchoolId.Value,
            created.InstructorId.Value,
            created.StudentId.Value,
            created.Route.ToDto(),
            created.Price.ToDto()
        ));
    }
    
    [HttpGet("/drivingLessons")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> GetDrivingLessonFromInstructor()
    {
        //TODO Authorization: Instructor should only be able to access their own lessons
        
        // Check Jwt Token to ascertain Instructor ID
        // Enforces that Instructor can only see lessons belonging to them
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        Instructor instructor;
        
        // Check that instructor exists in DB
        try
        {
            instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
        }
        catch (InstructorNotFoundException e)
        {
            return NotFound("Instructor not found. " + e.Message);
        }
        
        
        
        
        var data = await _drivingLessonService.GetAllDrivingLessonsFromInstructor(instructor.Id);
        return Ok(data.Select(x => new DrivingLessonDto(
            x.Id.Value,
            x.SchoolId.Value,
            x.InstructorId.Value,
            x.StudentId.Value,
            x.Route.ToDto(),
            x.Price.ToDto()
        )));
    }
    
    
    
}