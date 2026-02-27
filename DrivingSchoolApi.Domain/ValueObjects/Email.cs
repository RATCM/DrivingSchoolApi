using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Email : ValueObject
{
    public required string Address { get; init; }
    
    private Email() {} // EF
    
    public static Email Create(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new InvalidInputException("Email cannot be empty");
        if (!address.Contains('@'))
            throw new InvalidInputException("Email must be valid");

        return new Email
        {
            Address = address
        };
    }
}