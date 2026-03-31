using System.Security.Claims;
using DrivingSchoolApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Utils;

public static class ControllerExtensions
{
    extension(HttpContext context)
    {
        public Claim GetUserIdClaim()
        {
            return context.User.FindFirst(ClaimTypes.NameIdentifier)!;
        }
        
        public string GetUserRoleClaim()
        {
            return context.User.FindFirst(ClaimTypes.Role)?.Value!;
        }
    }

    extension(ControllerBase controller)
    {
        public ObjectResult Problem(Exception error)
        {
            return error switch
            {
                ApplicationException ex => controller.BadRequest(ex.Message),
                DomainException ex => controller.BadRequest(ex.Message),
                _ => controller.Problem("An unexpected error occurred.", statusCode: 500)
            };
        }
    }
    
}