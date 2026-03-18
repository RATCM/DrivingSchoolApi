using System.Security.Claims;

namespace DrivingSchoolApi.Utils;

public static class ControllerExtensions
{
    extension(HttpContext context)
    {
        public Guid GetUserIdClaim()
        {
            return new Guid(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }
    }
}