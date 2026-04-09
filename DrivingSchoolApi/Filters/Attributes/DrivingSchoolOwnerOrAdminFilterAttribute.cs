using DrivingSchoolApi.Filters.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Filters.Attributes;

public class DrivingSchoolOwnerOrAdminFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// When applied to an endpoint or controller, it checks
    /// if the user is either an owner of the driving school, or an admin
    /// </summary>
    /// <param name="key">The schools guid in the route template</param>
    /// <example>
    /// <code>
    /// [HttpPut("{id}")] // Route template
    /// [Authorize]
    /// [DrivingSchoolOwnerOrAdminFilterAttribute("id")] // Put the name of the id from the route template
    /// public async Task&lt;IActionResult&gt; UpdateDrivingSchool(Guid id, [FromBody] DrivingSchoolRegistry drivingSchool)
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// If the id is in the [Route] attribute instead (on the controller),
    /// then you should reference the id on the route attribute instead
    /// </remarks>
    public DrivingSchoolOwnerOrAdminFilterAttribute(string key) : base(typeof(DrivingSchoolOwnerOrAdminFilterService))
    {
        Arguments = [key];
        Order = 2;
    }
}
