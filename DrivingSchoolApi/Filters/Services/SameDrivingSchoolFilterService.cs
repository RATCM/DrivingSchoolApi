using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrivingSchoolApi.Filters.Services;

public enum TargetEntity
{
    Student,
    Instructor,
    School
}

public class SameDrivingSchoolFilterService : IAsyncResourceFilter
{
    private readonly string _key;
    private readonly TargetEntity _targetEntity;
    private readonly bool _letAdminsBypass;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;

    public SameDrivingSchoolFilterService(
        string key,
        TargetEntity targetEntity,
        bool letAdminsBypass,
        IStudentService studentService,
        IInstructorService instructorService)
    {
        _key = key;
        _targetEntity = targetEntity;
        _letAdminsBypass = letAdminsBypass;
        _studentService = studentService;
        _instructorService = instructorService;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // Validate that the URI-guid exists
        var providedIdStr = context.HttpContext.GetRouteValue(_key) as string;
        if (providedIdStr is null)
        {
            context.Result = CreateErrorResult(
            StatusCodes.Status500InternalServerError);
            return;
        }
        
        
        var callerIdClaim = context.HttpContext.GetUserIdClaim();
        if (callerIdClaim is null)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status401Unauthorized,
                "You need to be logged-in to access this resource"
                );
            return;
        }

        var callerId = new Guid(callerIdClaim.Value);
        
        var callerRole = context.HttpContext.GetUserRoleClaim();
        if (callerRole is null)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status401Unauthorized
                );
            return;
        }

        // Admin bypass
        if (_letAdminsBypass && callerRole == UserRole.Admin)
        {
            await next();
            return;
        } 
        else if (!_letAdminsBypass && callerRole == UserRole.Admin)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status401Unauthorized
                );
            return;
        }
        
        // Find requestee corresponding schoolId
        var callerSchoolResult = await GetSchoolIdForUser(callerRole.Value, callerId);
        
        if (!callerSchoolResult.IsSuccess)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status404NotFound,
                callerSchoolResult.Error!.Message
                );
            return;
        }
        
        // Find the targets corresponding schoolId
        var targetId = new Guid(providedIdStr);
        var targetSchoolResult = await GetSchoolIdForEntity(_targetEntity, targetId);
        
        if (!targetSchoolResult.IsSuccess)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status404NotFound,
                targetSchoolResult.Error!.Message);
            return;
        }

        var allowedAccess = callerSchoolResult.Value!.Value == targetSchoolResult.Value!.Value;
        if (!allowedAccess)
        {
            context.Result = CreateErrorResult(
                StatusCodes.Status401Unauthorized,
                "You don't have access to resources outside your school");
            return;
        }
        
        
        await next();
    }
    
    private async Task<Result<DrivingSchoolKey>> GetSchoolIdForUser(UserRole role, Guid id)
    {
        return role switch
        {
            UserRole.Student => await _studentService.GetStudentDrivingSchoolId(StudentKey.Create(id)),
            UserRole.Instructor => await _instructorService.GetInstructorDrivingSchoolId(InstructorKey.Create(id)),
            _ => new InvalidOperationException($"Unsupported role: {role}")
        };
    }
    
    private async Task<Result<DrivingSchoolKey>> GetSchoolIdForEntity(TargetEntity role, Guid id)
    {
        return role switch
        {
            TargetEntity.Student => await _studentService.GetStudentDrivingSchoolId(StudentKey.Create(id)),
            TargetEntity.Instructor => await _instructorService.GetInstructorDrivingSchoolId(InstructorKey.Create(id)),
            TargetEntity.School => DrivingSchoolKey.Create(id),
            _ => new InvalidOperationException($"Unsupported role: {role}")
        };
    }
    
    private static JsonResult CreateErrorResult(int statusCode)
    {
        return new JsonResult(new
        {
            Status = statusCode
        })
        {
            StatusCode = statusCode
        };
    }
    
    private static JsonResult CreateErrorResult(int statusCode, string message)
    {
        return new JsonResult(new
        {
            Status = statusCode,
            Message = message
        })
        {
            StatusCode = statusCode
        };
    }
}
