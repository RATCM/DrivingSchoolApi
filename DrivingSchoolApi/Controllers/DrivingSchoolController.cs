using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.ValueObject;
using DrivingSchoolApi.Filters.Attributes;
using DrivingSchoolApi.Filters.Services;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DrivingSchoolController : ControllerBase
{
    private readonly IDrivingSchoolService _drivingSchoolService;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;
    private readonly ICompletedCourseService _completedCourseService;

    public DrivingSchoolController(
        ILogger<DrivingSchoolController> logger,
        IDrivingSchoolService drivingSchoolService,
        IStudentService studentService,
        IInstructorService instructorService,
        ICompletedCourseService completedCourseService)
    {
        _drivingSchoolService = drivingSchoolService;
        _studentService = studentService;
        _instructorService = instructorService;
        _completedCourseService = completedCourseService;
    }
    
    
    //TODO Add paging
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetAllDrivingSchools()
    {
        var result = await _drivingSchoolService.GetAllDrivingSchools();

        return result.IsSuccess
            ? Ok(result.Value!.Select(x => x.ToDto()))
            : this.Problem(result.Error!);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<DrivingSchoolDto>>> GetDrivingSchool(Guid id)
    {
        var result = await _drivingSchoolService.GetDrivingSchoolById(DrivingSchoolKey.Create(id));
        
        return result.IsSuccess
            ? Ok(result.Value!.ToDto())
            : this.Problem(result.Error!);
    }
    
        
    [HttpGet("{id}/rating")]
    public async Task<IActionResult> GetDrivingSchoolRating(Guid id)
    {
        var courses = await _completedCourseService.GetAllCompletedCoursesFromSchool(DrivingSchoolKey.Create(id));
        if (!courses.IsSuccess) 
            return this.Problem(courses.Error!);

        // Ensure that we aren't dividing by zero
        var numTotal = courses.Value!.Count != 0 ? courses.Value!.Count : 1;
        var numPasses = courses.Value!.Count(x => x.Reason == CourseCompletionReason.Finished);
        var numFail = courses.Value!.Count(x => x.Reason == CourseCompletionReason.Failed);
        var numQuit = courses.Value!.Count(x => x.Reason == CourseCompletionReason.Quit);

        // .Average() throws an exception if the collection is empty
        var avgPrice = courses.Value.Count != 0 ? courses.Value!.Select(x => x.Cost.Amount).Average() : 0;

        return Ok(new DrivingSchoolRatingDto(
            (float)numPasses/numTotal,
            (float)numFail/numTotal,
            (float)numQuit/numTotal,
            new MoneyDto(avgPrice, "DKK")));
    }
    
    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<IActionResult> CreateDrivingSchool([FromBody] DrivingSchoolRegistryDto drivingSchool)
    {
        var result = await _drivingSchoolService.CreateDrivingSchool(
            DrivingSchoolName.Create(drivingSchool.Name),
            StreetAddress.Create("N/A", "N/A", "N/A", drivingSchool.Address),
            PhoneNumber.Create(drivingSchool.PhoneNumber),
            WebAddress.Create(drivingSchool.WebAddress),
            []);
        
        return result.IsSuccess
            ? Created($"theoryLesson/{result.Value!.Id}", result.Value.ToDto())
            : this.Problem(result.Error!);
    }
    
    //TODO Add paging
    [HttpGet("{schoolId:guid}/students")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    [SameDrivingSchoolFilter("schoolId", TargetEntity.School)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudentFromSchool(Guid schoolId)
    {
        var result = await _studentService.GetAllStudentsFromSchool(DrivingSchoolKey.Create(schoolId));
        
        return result.IsSuccess
            ? Ok(result.Value!.Select(s => s.ToDto()))
            : BadRequest("Failed to retrieve students.");
    }
    
    [HttpPost("{schoolId:guid}/student/invite")]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<ActionResult<StudentInviteDto>> CreateInvite(Guid schoolId)
    {
        var idClaim = Guid.Parse(HttpContext.GetUserIdClaim()!.Value);

        var instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));

        if (instructor.IsSuccess)
            return this.Problem(instructor.Error!);
        
        var invite = await _drivingSchoolService.CreateStudentInvite(
            DrivingSchoolKey.Create(schoolId), 
            TimeSpan.FromDays(30)); // We just have the invite be available for 30 days for now

        return invite.IsSuccess
            ? Ok(invite.Value!)
            : this.Problem(invite.Error!);
    }
}
