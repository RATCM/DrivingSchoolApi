using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record PasswordHash : ValueObject
{
    public string Hash { get; }

    private PasswordHash() {} // EF
    public PasswordHash(string hash)
    {
        if (string.IsNullOrEmpty(hash))
            throw new InvalidInputException("Password hash cannot be empty");
        
        Hash = hash;
    }
}