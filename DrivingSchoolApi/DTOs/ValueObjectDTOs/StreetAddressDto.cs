namespace DrivingSchoolApi.DTOs;

public record StreetAddressDto(
    string PostalCode,
    string City,
    string Region,
    string AddressLine);