using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Email : ValueObject
{
    public string Address { get; }
    
    private Email() {} // EF

    public Email(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new InvalidInputException("Email cannot be empty");
        if (!address.Contains('@'))
            throw new InvalidInputException("Email must be valid");

        Address = address;
    }
}