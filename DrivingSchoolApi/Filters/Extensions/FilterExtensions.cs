using System.Text.RegularExpressions;

namespace DrivingSchoolApi.Filters.Extensions;

public static partial class FilterExtensions
{
    extension(IFilter filter)
    {
        public static string RouteToKey(string route)
        {
            var match = MyRegex().Match(route);
            if (!match.Success)
                throw new Exception("Invalid route parameter");
            // Only one can be empty at the same time
            return !string.IsNullOrEmpty(match.Groups[1].Value)
                ? match.Groups[1].Value
                : match.Groups[2].Value;
        }

    }
    
    [GeneratedRegex(@"^(?:{([a-zA-Z_]\w*)(?:\:[a-zA-Z_]\w*)?})$|^([a-zA-Z_]\w*)$")]
    private static partial Regex MyRegex();
}