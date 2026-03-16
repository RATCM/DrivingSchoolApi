using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IStudentService _studentService;

    public StudentController(
        ILogger<InstructorController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService)
    {
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
    }
    
    [HttpGet]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    public async Task<IActionResult> GetTheoryLessonsFromStudent()
    {
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
}