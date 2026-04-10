using System.Net;
using DrivingSchoolApi.Models;
using DrivingSchoolApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrivingSchoolApi.Filters.Attributes;

public class UserFilterAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    private readonly string _key;
    private readonly bool _letAdminsBypass;
    
    /// <summary>
    /// When applied to an endpoint or controller, it enforces TokenUserID == EndpointID
    /// </summary>
    /// <param name="key">The user guid in the route template</param>
    /// <param name="letAdminsBypass">Whether admins should be allowed to bypass this restriction</param>
    /// <example>
    /// <code>
    /// [HttpPut("{id}")] // Route template
    /// [Authorize]
    /// [UserFilter("id")] // Put the name of the id from the route template
    /// public async Task&lt;IActionResult&gt; UpdateUser(Guid id, [FromBody] UserRegistry user)
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// If the id is in the [Route] attribute instead (on the controller),
    /// then you should reference the id on the route attribute instead
    /// </remarks>
    public UserFilterAttribute(string key, bool letAdminsBypass = false)
    {
        _key = key;
        _letAdminsBypass = letAdminsBypass;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Console.WriteLine("UserFilter...");
        var userIdClaim = context.HttpContext.GetUserIdClaim();
        //Console.WriteLine($"Id claim: {userIdClaim?.Value}");
        if (userIdClaim is null)
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

        var userId = new Guid(userIdClaim.Value);
        var providedIdStr = context.HttpContext.GetRouteValue(_key) as string;
        //Console.WriteLine($"Provided id str: {providedIdStr}");
        if (providedIdStr is null)
        {
            var errorResponse = new
            {
                Status = (int)HttpStatusCode.BadRequest,
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
            return;
        }

        var isAdmin = context.HttpContext.User.IsInRole(nameof(UserRole.Admin));
        var providedId = new Guid(providedIdStr);
        //Console.WriteLine(isAdmin);

        var allowedAccess = userId == providedId || (isAdmin && _letAdminsBypass);
        if ( !allowedAccess )
        {
            var errorResponse = new
            {
                Status = (int)HttpStatusCode.Forbidden,
                Message = "You are not allowed to update other users resources"
            };
            
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
            };
            
            return;
        }
        
        await next();
    }
}

