using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record PhoneNumber : ValueObject
{
    public required string Number { get; init; }
    
    private PhoneNumber() {} // EF
    
    public static PhoneNumber Create(string number)
    {
        if (string.IsNullOrEmpty(number))
            throw new InvalidInputException("Phone number cannot be empty");

        return new PhoneNumber
        {
            Number = number
        };
    }
}