namespace DrivingSchoolApi.Application.Auth;

public static class AuthPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string InstructorOnly = "InstructorOnly";
    public const string StudentOnly = "StudentOnly";
}