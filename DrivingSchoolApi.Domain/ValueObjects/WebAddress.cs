using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.ValueObjects;

public record WebAddress : ValueObject
{
    public string Url { get; }
    
    private WebAddress() {} // EF

    public WebAddress(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new InvalidInputException("Url cannot be empty");
        if (!url.Contains('.')) // Very basic Url validation
            throw new InvalidInputException("Url must be valid");
        
        Url = url;
    }
}