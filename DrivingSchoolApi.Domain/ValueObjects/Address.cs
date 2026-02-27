using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record Address : ValueObject
{
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string Region { get; init; }
    public required string AddressLine { get; init; }
    
    private Address() {} // EF
    
    public static Address Create(
        string postalCode, 
        string city, 
        string region, 
        string addressLine)
    {
        if (string.IsNullOrEmpty(postalCode))
            throw new InvalidInputException("Postal code cannot be empty");
        if (string.IsNullOrEmpty(city))
            throw new InvalidInputException("City cannot be empty");
        if (string.IsNullOrEmpty(region))
            throw new InvalidInputException("Region cannot be empty");
        if (string.IsNullOrEmpty(addressLine))
            throw new InvalidInputException("Address line cannot be empty");

        return new Address
        {
            PostalCode = postalCode,
            City = city,
            Region = region,
            AddressLine = addressLine
        };
    }
}