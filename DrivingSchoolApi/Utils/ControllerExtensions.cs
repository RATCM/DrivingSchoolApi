using System.Security.Claims;
using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = DrivingSchoolApi.Application.Exceptions.ApplicationException;

namespace DrivingSchoolApi.Utils;

public static class ControllerExtensions
{
    extension(HttpContext context)
    {
        public Claim? GetUserIdClaim()
        {
            return context.User.FindFirst(ClaimTypes.NameIdentifier);
        }
        
        public UserRole? GetUserRoleClaim()
        {
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
            return role is null
                ? null
                : Enum.Parse<UserRole>(role);
        }
    }

    extension(ControllerBase controller)
    {
        public ObjectResult Problem(Exception error)
        {
            return error switch
            {
                ApplicationException ex => controller.Problem(ex.Message, statusCode: ex.ResponseCode),
                DomainException ex => controller.BadRequest(ex.Message),
                _ => controller.Problem("An unexpected error occurred.", statusCode: 500)
            };
        }
    }
    
}