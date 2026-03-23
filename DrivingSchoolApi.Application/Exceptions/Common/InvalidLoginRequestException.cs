namespace DrivingSchoolApi.Application.Exceptions.Common;

public class InvalidLoginRequestException : BadRequestException
{
    public InvalidLoginRequestException() : base("Email or password is incorrect.") { }
}