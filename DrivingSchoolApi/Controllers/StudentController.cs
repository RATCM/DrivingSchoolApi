using System.Security.Claims;
using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.CompletedCourse;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.Filters.Attributes;
using DrivingSchoolApi.Filters.Services;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IStudentService _studentService;
    private readonly IStudentInviteService _studentInviteService;
    private readonly ICompletedCourseService _completedCourseService;

    public StudentController(
        ILogger<StudentController> logger,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        IStudentService studentService,
        IStudentInviteService studentInviteService,
        ICompletedCourseService completedCourseService)
    {
        _logger = logger;
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _studentService = studentService;
        _studentInviteService = studentInviteService;
        _completedCourseService = completedCourseService;
    }
    
    //TODO login
    [HttpPost("login")]
    public async Task<ActionResult> LoginAsStudent([FromBody] StudentLoginRequestDto loginRequest)
    {
        var result = await _studentService.LoginAsStudent(loginRequest.Email, loginRequest.Password);
        
        return result.IsSuccess
            ? Ok(new JwtTokenDto{AccessToken = result.Value!.AccessToken, RefreshToken = result.Value.RefreshToken})
            : this.Problem(result.Error!, _logger);
    }
    
    //TODO register (should be implemented studentInvite branch)
    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents(int page = 1)
    {
        var result = await _studentService.GetAllStudents();
        const int PAGE_SIZE = 30;
        return result.IsSuccess
            ? Ok(result.Value!.Skip(PAGE_SIZE*(page-1)).Take(PAGE_SIZE).Select(x => x.ToDto()))
            : this.Problem(result.Error!, _logger);
    }
    
    
    [HttpGet("{studentId:guid}/theoryLesson")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    [UserFilter("{studentId:guid}")]
    public async Task<IActionResult> GetTheoryLessonsFromStudent(Guid studentId)
    {
        var result = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(studentId));
        
        return result.IsSuccess ?
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!, _logger);
    }

    
    [HttpGet("{studentId:guid}/drivingLesson")]
    [Authorize(Policy = AuthPolicies.StudentOnly)]
    [UserFilter("{studentId:guid}")]
    public async Task<IActionResult> GetDrivingLessonsFromStudent(Guid studentId)
    {
        var result = await _drivingLessonService.GetAllDrivingLessonsFromStudent(StudentKey.Create(studentId));

        return result.IsSuccess ? 
            Ok(result.Value!.Select(x => x.ToDto())) : 
            this.Problem(result.Error!, _logger);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRegistryDto student)
    {
        var studentInviteResult = await _studentInviteService.RedeemStudentInvite(
            StudentInviteKey.Create(student.InviteId));

        if (!studentInviteResult.IsSuccess)
            return this.Problem(studentInviteResult.Error!);
        
        var result = await _studentService.CreateStudent(
            Name.Create(student.StudentName.FirstName, student.StudentName.LastName),
            Email.Create(student.EmailAddress),
            student.Password,
            PhoneNumber.Create(student.PhoneNumber),
            studentInviteResult.Value!.Id);
        
        var created = result.Value!;

        return result.IsSuccess ?
            Created($"student/{created.Id}", result.Value!.ToDto()) :
            this.Problem(result.Error!, _logger);
    }

    
    [HttpDelete("{studentId:Guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrStudent)]
    [UserFilter("{studentId:guid}", letAdminsBypass: true)]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {

        var deleted = await _studentService.DeleteStudent(StudentKey.Create(studentId));

        return deleted.IsSuccess ? 
            NoContent() : 
            this.Problem(deleted.Error!, _logger);
    }
    
    
    [HttpGet("{studentId:guid}")]
    [Authorize(Policy = AuthPolicies.AdminOrInstructor)]
    [SameDrivingSchoolFilter("{studentId:guid}", TargetEntity.Student,true)]
    public async Task<ActionResult<StudentDto>> GetStudentById(Guid studentId)
    {
        var student = await _studentService.GetStudentById(StudentKey.Create(studentId));
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(StudentKey.Create(studentId));
        var drivingLessons = await _drivingLessonService.GetAllDrivingLessonsFromStudent(StudentKey.Create(studentId));

        return student.IsSuccess ?
            Ok(student.Value!.ToDto(theoryLessons: theoryLessons.Value, drivingLessons: drivingLessons.Value)) :
            this.Problem(student.Error!, _logger);
    }

    [HttpGet("{studentId:guid}/course/{courseId:guid}")]
    [Authorize]
    [UserFilter("{studentId:guid}")]
    public async Task<IActionResult> GetCompletedCourseById(Guid studentId, Guid courseId)
    {
        var course = await _completedCourseService.GetCompletedCourseById(CompletedCourseKey.Create(courseId));

        if (!course.IsSuccess)
            return this.Problem(course.Error!, _logger);

        if (!StudentKey.Create(studentId).Equals(course.Value!.StudentId))
            return Forbid();

        return Ok(course);
    }
    
    [HttpGet("{studentId:guid}/course")]
    [Authorize]
    [UserFilter("{studentId:guid}")]
    public async Task<IActionResult> GetAllCompletedCourses(Guid studentId)
    {
        var courses = await _completedCourseService.GetAllCompletedCoursesFromStudent(StudentKey.Create(studentId));

        if (!courses.IsSuccess)
            return this.Problem(courses.Error!, _logger);
        
        return Ok(courses);
    }

    [HttpPost("{studentId:guid}/course")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [SameDrivingSchoolFilter("{studentId:guid}", TargetEntity.Student)]
    public async Task<IActionResult> CompleteCourse(Guid studentId, [FromBody] CompletedCourseRegistryDto registry)
    {
        var parsed = Enum.TryParse<CourseCompletionReason>(registry.Reason, out var reason);
        if (!parsed) return BadRequest("Reason must be a valid value");
        
        var result = await _completedCourseService.CreateCompletedCourseForStudent(
            StudentKey.Create(studentId),
            registry.IncludeLessonsFrom,
            reason);

        return result.IsSuccess
            ? Created($"student/{studentId}/course/{result.Value!.Id.Value}", result.Value!)
            : this.Problem(result.Error!, _logger);
    }
    
    
}
