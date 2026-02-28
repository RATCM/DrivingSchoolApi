using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record StreetAddress : ValueObject
{
    public string PostalCode { get; }
    public string City { get; }
    public string Region { get; }
    public string AddressLine { get; }
    
    private StreetAddress() {} // EF

    public StreetAddress(string postalCode, string city, string region, string addressLine)
    {
        if (string.IsNullOrEmpty(postalCode))
            throw new InvalidInputException("Postal code cannot be empty");
        if (string.IsNullOrEmpty(city))
            throw new InvalidInputException("City cannot be empty");
        if (string.IsNullOrEmpty(region))
            throw new InvalidInputException("Region cannot be empty");
        if (string.IsNullOrEmpty(addressLine))
            throw new InvalidInputException("Street Address line cannot be empty");
        
        PostalCode = postalCode;
        City = city;
        Region = region;
        AddressLine = addressLine;
    }
}