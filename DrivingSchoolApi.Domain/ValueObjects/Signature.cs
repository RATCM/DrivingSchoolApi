using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Signature : ValueObject
{
    public required byte[] Blob { get; init; }
    private Signature() {} // EF

    public static Signature Create(byte[] blob)
    {
        if (blob.Length == 0)
            throw new InvalidInputException("Signature cannot be empty");

        return new Signature()
        {
            Blob = blob
        };
    }
}