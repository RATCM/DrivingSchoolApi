using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money() {} // EF
    
    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new InvalidInputException("Money amount cannot be negative");
        if (string.IsNullOrEmpty(currency)) throw new InvalidInputException("Currency cannot be null or empty");

        Amount = amount;
        Currency = currency;
    }
}