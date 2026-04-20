using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.DTOs.Student;
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

    public StudentInviteController(
        IInstructorService instructorService,
        IDrivingSchoolService drivingSchoolService)
    {
        _instructorService = instructorService;
        _drivingSchoolService = drivingSchoolService;
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
    //GetDrivingSchoolInvites
    //GetDrivingSchoolInviteById
    //InvalidateInviteById
}