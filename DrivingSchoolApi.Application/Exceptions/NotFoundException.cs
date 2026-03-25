namespace DrivingSchoolApi.Application.Exceptions;

public abstract class NotFoundException : ApplicationException
{
    protected NotFoundException(string message) : base(404, message) { }
}