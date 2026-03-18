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

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] InstructorLoginRequestDto loginRequest)
    {
        //TODO
        throw new NotImplementedException();
    }

    [HttpGet("all")]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<InstructorDto>>> GetAllInstructors()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    [HttpPost("/create/theoryLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateTheoryLesson([FromBody] TheoryLessonRegistryDto theoryLessonRegistryDto)
    {
        
        //TODO Authorization: Instructor should only be able to create lessons for themselves and their own school
        
        var result = await _theoryLessonService.CreateTheoryLesson(
            DrivingSchoolKey.Create(theoryLessonRegistryDto.SchoolId),
            theoryLessonRegistryDto.LessonDateTime,
            Money.Create(theoryLessonRegistryDto.Price.Amount, theoryLessonRegistryDto.Price.Currency),
            InstructorKey.Create(theoryLessonRegistryDto.InstructorId),
            theoryLessonRegistryDto.StudentIds.Select(StudentKey.Create).ToList());

        if (!result.IsSuccess)
        {
            BadRequest("Theory lesson creation failed.");
        }
        
        var created = result.Value!;
        
        return Created($"theoryLesson/{created.Id}", new TheoryLessonDto(
            created.Id.Value,
            created.SchoolId.Value,
            created.InstructorId.Value,
            created.LessonDateTime,
            created.Price.ToDto(),
            created.StudentIds.Select(studentKey => studentKey.Value).ToList()
            ));
    }
    
    [HttpGet("/theoryLessons")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromInstructor()
    {
        // Check Jwt Token to ascertain Instructor ID
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await _theoryLessonService.GetAllTheoryLessonsFromInstructor(InstructorKey.Create(idClaim));
        if (!result.IsSuccess) { return BadRequest("Failed to retrieve theory lessons."); }
        var theoryLessons = result.Value!;
        
        return Ok(theoryLessons.Select(x => new TheoryLessonDto(
            x.Id.Value,
            x.SchoolId.Value,
            x.InstructorId.Value,
            x.LessonDateTime,
            x.Price.ToDto(),
            x.StudentIds.Select(studentKey => studentKey.Value).ToList()
        )));
    }
    
    [HttpPost("create/drivingLesson")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<IActionResult> CreateDrivingLesson([FromBody] DrivingLessonRegistryDto registryDto)
    {
        // Check Jwt Token to ascertain Instructor ID
        // Enforces that Instructor can only create lessons for themselves
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var instructorResult = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
        if (!instructorResult.IsSuccess) { return BadRequest("Failed to retrieve instructor."); }
        var instructor = instructorResult.Value!;
        
        var studentResult = await _studentService.GetStudentById(StudentKey.Create(registryDto.StudentId));
        if (!studentResult.IsSuccess) { return BadRequest("Failed to retrieve student."); }
        var student = studentResult.Value!;
        

        // Check that student is from the same school as instructor
        if (student.SchoolId.Value != instructor.SchoolId.Value)
        {
            return BadRequest("Student is not assigned to the same school as the instructor.");
        }
        
        //TODO check that information is correct (e.g. price is not negative, route is valid etc.)
        
        var createdResult = await _drivingLessonService.CreateDrivingLesson(
            instructor.SchoolId,
            registryDto.Route.ToDomain(),
            registryDto.Price.ToDomain(),
            instructor.Id,
            StudentKey.Create(registryDto.StudentId));
        var created = createdResult.Value!;
        
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
        // Check Jwt Token to ascertain Instructor ID
        // Enforces that Instructor can only see lessons belonging to them
        var idClaim = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var instructorResult = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
        if (!instructorResult.IsSuccess) { return BadRequest("Failed to retrieve instructor."); }
        var instructor = instructorResult.Value!;
        
        var dataResult = await _drivingLessonService.GetAllDrivingLessonsFromInstructor(instructor.Id);
        if (!dataResult.IsSuccess) { return BadRequest("Failed to retrieve driving lessons."); }
        var data = dataResult.Value!;
        
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
