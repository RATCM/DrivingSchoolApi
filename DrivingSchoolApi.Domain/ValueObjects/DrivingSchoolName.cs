using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record DrivingSchoolName : ValueObject
{
    public required string Name { get; init; }
    
    private DrivingSchoolName() {} // EF

    public static DrivingSchoolName Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidInputException("Driving School name cannot be empty");
        
        return new DrivingSchoolName
        {
            Name = name
        };
    }
}