using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record PhoneNumber : ValueObject
{
    public string Number { get; }
    
    private PhoneNumber() {} // EF

    public PhoneNumber(string number)
    {
        if (string.IsNullOrEmpty(number))
            throw new InvalidInputException("Phone number cannot be empty"); 
        
        Number = number;
    }
}