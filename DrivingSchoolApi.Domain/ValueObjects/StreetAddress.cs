using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record StreetAddress : ValueObject
{
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string Region { get; init; }
    public required string AddressLine { get; init; }
    
    private StreetAddress() {} // EF
    
    public static StreetAddress Create(
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
            throw new InvalidInputException("Street Address line cannot be empty");

        return new StreetAddress
        {
            PostalCode = postalCode,
            City = city,
            Region = region,
            AddressLine = addressLine
        };
    }
}