namespace DrivingSchoolApi.Application.Exceptions;

public abstract class BadRequestException : ApplicationException
{
    protected BadRequestException(string message) : base(400, message) { }
}