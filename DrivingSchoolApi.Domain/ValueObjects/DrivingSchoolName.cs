using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record DrivingSchoolName : ValueObject
{
    public string Name { get; }
    
    private DrivingSchoolName() {} // EF

    public DrivingSchoolName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidInputException("Driving School name cannot be empty");
        Name = name;
    }
}