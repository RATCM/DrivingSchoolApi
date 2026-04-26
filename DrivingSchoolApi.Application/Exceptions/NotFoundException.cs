namespace DrivingSchoolApi.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    protected NotFoundException(string message) : base(404, message) { }
}