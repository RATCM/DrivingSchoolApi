using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record PasswordHash : ValueObject
{
    public required string Hash { get; init; }

    private PasswordHash() {} // EF
    
    public static PasswordHash Create(string hash)
    {
        if (string.IsNullOrEmpty(hash))
            throw new InvalidInputException("Password hash cannot be empty");

        return new PasswordHash
        {
            Hash = hash
        };
    }
}