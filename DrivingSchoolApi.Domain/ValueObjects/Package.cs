using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Package : ValueObject
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required Money Price { get; init; }
    
    private Package() {}

    public static Package Create(string title, string description, Money price)
    {
        if (string.IsNullOrEmpty(title))
            throw new InvalidInputException("Package title cannot be empty");

        if (string.IsNullOrEmpty(description))
            throw new InvalidInputException("Package description cannot be empty");
        
        return new Package
        {
            Title = title,
            Description = description,
            Price = price
        };
    }
}