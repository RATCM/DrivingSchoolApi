namespace DrivingSchoolApi.Application.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string message) : base(400, message) { }
}