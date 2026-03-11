namespace DrivingSchoolApi.Domain.Exceptions;

public class InvalidInputException : DomainException
{
    public InvalidInputException(string message) : base(message) { }
}