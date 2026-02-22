using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Name : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    
    private Name() {} // EF

    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new InvalidInputException("First name cannot be empty");
        if (string.IsNullOrEmpty(lastName))
            throw new InvalidInputException("Last name cannot be empty");

        FirstName = firstName;
        LastName = lastName;
    }
}