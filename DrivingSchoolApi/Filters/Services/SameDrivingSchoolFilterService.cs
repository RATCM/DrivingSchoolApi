using System.Net;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Models;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrivingSchoolApi.Filters.Services;

public class SameDrivingSchoolFilterService : IAsyncResourceFilter
{
    private readonly string _key;
    private readonly UserRole _targetRole;
    private readonly bool _letAdminsBypass;
    private readonly IStudentService _studentService;
    private readonly IInstructorService _instructorService;
    private readonly ILogger<SameDrivingSchoolFilterService> _logger;

    public SameDrivingSchoolFilterService(
        string key,
        UserRole targetRole,
        UserRole requesteeRole,
        bool letAdminsBypass,
        IStudentService studentService,
        IInstructorService instructorService,
        ILogger<SameDrivingSchoolFilterService> logger)
    {
        _key = key;
        _targetRole = targetRole;
        _letAdminsBypass = letAdminsBypass;
        _studentService = studentService;
        _instructorService = instructorService;
        _logger = logger;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // Validate that the URI-guid exists
        var providedIdStr = context.HttpContext.GetRouteValue(_key) as string;
        if (providedIdStr is null)
        {
            var errorResponse = new
            {
                Status = (int)HttpStatusCode.InternalServerError,
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            return;
        }
        
        
        var requesteeIdClaim = context.HttpContext.GetUserIdClaim();
        if (requesteeIdClaim is null)
        {
            var errorResponse = new
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = "You need to be logged-in to access this resource"
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
            return;
        }

        var requesteeId = new Guid(requesteeIdClaim.Value);
        
        var requesteeRoleClaim = context.HttpContext.GetUserRoleClaim();
        if (requesteeRoleClaim is null)
        {
            var errorResponse = new
            {
                Status = (int)HttpStatusCode.Unauthorized,
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
            return;
        }

        // Admin bypass
        if (_letAdminsBypass && requesteeRoleClaim == nameof(UserRole.Admin))
        {
            await next();
            return;
        } 
        else if (!_letAdminsBypass && requesteeRoleClaim == nameof(UserRole.Admin))
        {
            context.Result = new JsonResult(new
            {
                Status = StatusCodes.Status401Unauthorized,
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        
        // Find requestee corresponding schoolId
        var requesteeSchoolResult = requesteeRoleClaim switch
        {
            nameof(UserRole.Student) => await _studentService.GetStudentDrivingSchoolId(StudentKey.Create(requesteeId)),
            nameof(UserRole.Instructor) => await _instructorService.GetInstructorDrivingSchoolId(InstructorKey.Create(requesteeId)),
            _ => throw new Exception("Invalid target role: " + requesteeRoleClaim)
        };
        
        if (!requesteeSchoolResult.IsSuccess)
        {
            context.Result = new JsonResult(new
            {
                Status = StatusCodes.Status404NotFound,
                Message = requesteeSchoolResult.Error!.Message
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            return;
        }
        
        var requesteeSchoolId = requesteeSchoolResult.Value!;
        
        
        // Find the targets corresponding schoolId
        var targetId = new Guid(providedIdStr);
        var targetSchoolResult = _targetRole switch
        {
            UserRole.Student => await _studentService.GetStudentDrivingSchoolId(StudentKey.Create(targetId)),
            UserRole.Instructor => await _instructorService.GetInstructorDrivingSchoolId(InstructorKey.Create(targetId)),
            _ => throw new Exception("Invalid target role: " + requesteeRoleClaim)
        };
        
        if (!targetSchoolResult.IsSuccess)
        {
            context.Result = new JsonResult(new
            {
                Status = StatusCodes.Status404NotFound,
                Message = targetSchoolResult.Error!.Message
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            return;
        }

        var targetSchoolId = targetSchoolResult.Value!;

        var allowedAccess = requesteeSchoolId.Value == targetSchoolId.Value;
        if (!allowedAccess)
        {
            context.Result = new JsonResult(new
            {
                Status = StatusCodes.Status401Unauthorized,
                Message = "You don't have access to resources outside your school"
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        
        
        await next();
    }
}
