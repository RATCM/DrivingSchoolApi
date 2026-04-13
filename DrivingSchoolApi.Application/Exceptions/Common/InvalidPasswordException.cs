namespace DrivingSchoolApi.Application.Exceptions.Common;

public class InvalidPasswordException : BadRequestException
{
    public InvalidPasswordException(string reason = "Invalid password.") : base(reason) { }
}