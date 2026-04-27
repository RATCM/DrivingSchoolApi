using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.Mappers;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Controllers;

[ApiController]
[Route("drivingSchool/{drivingSchoolId:guid}/student/invite")]
public class StudentInviteController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    private readonly IDrivingSchoolService _drivingSchoolService;
    private readonly IStudentInviteService _studentInviteService;

    public StudentInviteController(
        IInstructorService instructorService,
        IDrivingSchoolService drivingSchoolService,
        IStudentInviteService studentInviteService)
    {
        _instructorService = instructorService;
        _drivingSchoolService = drivingSchoolService;
        _studentInviteService = studentInviteService;
    }
    
    [HttpPost]
    [Authorize(Policy = AuthPolicies.InstructorOnly)]
    public async Task<ActionResult<StudentInviteDto>> CreateInvite(Guid schoolId)
    {
        var idClaim = Guid.Parse(HttpContext.GetUserIdClaim()!.Value);

        var instructor = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));

        if (!instructor.IsSuccess)
            return this.Problem(instructor.Error!);
        
        var invite = await _drivingSchoolService.CreateStudentInvite(
            DrivingSchoolKey.Create(schoolId), 
            TimeSpan.FromDays(30)); // We just have the invite be available for 30 days for now

        return invite.IsSuccess
            ? Ok(invite.Value!)
            : this.Problem(invite.Error!);
    }
    
    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOnly)]
    public async Task<ActionResult> GetAllDrivingSchoolInvites(int page = 1)
    {
        var result = await _studentInviteService.GetAll();
        const int PAGE_SIZE = 30;
        
        return result.IsSuccess
            ? Ok(result.Value!.Skip(PAGE_SIZE*(page-1)).Take(PAGE_SIZE).Select(x => x.ToDto()))
            : this.Problem(result.Error!);
    }
    //GetDrivingSchoolInvites
    //GetDrivingSchoolInviteById
    //InvalidateInviteById
}