using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Money : ValueObject
{
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }

    private Money() {} // EF

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0) throw new InvalidInputException("Money amount cannot be negative");
        if (string.IsNullOrEmpty(currency)) throw new InvalidInputException("Currency cannot be null or empty");

        return new Money
        {
            Amount = amount,
            Currency = currency
        };
    }
}