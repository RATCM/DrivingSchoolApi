namespace DrivingSchoolApi.DTOs.ValueObject;

public record StreetAddressDto(
    string PostalCode,
    string City,
    string Region,
    string AddressLine);
    