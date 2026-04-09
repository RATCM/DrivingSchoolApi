using DrivingSchoolApi.Filters.Services;
using DrivingSchoolApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApi.Filters.Attributes;

public class SameDrivingSchoolFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// When applied to an endpoint or controller, it checks
    /// if the userId is connected to the same school as the URI id.
    /// </summary>
    /// <param name="key">The targets guid in the route template</param>
    /// <param name="allowAdmins">Whether admins should be allowed to bypass this restriction</param>
    /// <example>
    /// <code>
    /// [HttpPut("{id}")] // Route template
    /// [Authorize]
    /// [SameDrivingSchoolFilterAttribute("id")] // Put the name of the id from the route template
    /// public async Task&lt;IActionResult&gt; UpdateStudent(Guid id, [FromBody] StudentRegistry student)
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// If the id is in the [Route] attribute instead (on the controller),
    /// then you should reference the id on the route attribute instead
    /// </remarks>
    public SameDrivingSchoolFilterAttribute(string key, UserRole targetRole,UserRole requesteeRole, bool allowAdmins) : base(typeof(SameDrivingSchoolFilterService))
    {
        Arguments = [key, targetRole, requesteeRole, allowAdmins];
        Order = 2;
    }
}
