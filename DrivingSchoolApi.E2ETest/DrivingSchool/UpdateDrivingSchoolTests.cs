using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.ValueObject;
using DrivingSchoolApi.Test.Extensions.Dtos;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

public class UpdateDrivingSchoolTests : TestClass
{
    [Test]
    public async Task UpdateDrivingSchool_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        
        var updateDto = new DrivingSchoolUpdateDto(
            "Updated School Name",
            new StreetAddressDto("2900", "Copenhagen", "Hovedstaden", "Strøget"),
            "98765432",
            "updated.school.com");
        
        // Act
        var updateResponse = await DrivingSchoolService.UpdateDrivingSchool(createdSchool!.Id, updateDto);
        
        // Assert
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var updatedSchool = await updateResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        Assert.That(updatedSchool!.Name, Is.EqualTo("Updated School Name"));
        Assert.That(updatedSchool.PhoneNumber, Is.EqualTo("98765432"));
        Assert.That(updatedSchool.WebAddress, Is.EqualTo("updated.school.com"));
    }
    
    [Test]
    public async Task UpdateDrivingSchool_FailsWhenSchoolNotFound()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        var nonExistentSchoolId = Guid.NewGuid();
        
        var updateDto = new DrivingSchoolUpdateDto(
            "Updated School Name",
            new StreetAddressDto("2900", "Copenhagen", "Hovedstaden", "Strøget"),
            "98765432",
            "updated.school.com");
        
        // Act
        var updateResponse = await DrivingSchoolService.UpdateDrivingSchool(nonExistentSchoolId, updateDto);
        
        // Assert
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task UpdateDrivingSchool_FailsWhenUnauthorized()
    {
        // Arrange - Don't login, so no bearer token
        await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        
        var updateDto = new DrivingSchoolUpdateDto(
            "Updated School Name",
            new StreetAddressDto("2900", "Copenhagen", "Hovedstaden", "Strøget"),
            "98765432",
            "updated.school.com");
        
        // Act
        var updateResponse = await DrivingSchoolService.UpdateDrivingSchool(Guid.NewGuid(), updateDto);
        
        // Assert
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task UpdateDrivingSchool_UpdatesOnlyModifiedFields()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        var originalStreetAddress = createdSchool!.StreetAddress;
        
        var updateDto = new DrivingSchoolUpdateDto(
            "Only Name Changed",
            originalStreetAddress,
            createdSchool.PhoneNumber,
            createdSchool.WebAddress);
        
        // Act
        var updateResponse = await DrivingSchoolService.UpdateDrivingSchool(createdSchool.Id, updateDto);
        
        // Assert
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var updatedSchool = await updateResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        Assert.That(updatedSchool!.Name, Is.EqualTo("Only Name Changed"));
        var streetAddress = updatedSchool.StreetAddress;
        Assert.That(streetAddress.PostalCode, Is.EqualTo(originalStreetAddress.PostalCode));
        Assert.That(streetAddress.City, Is.EqualTo(originalStreetAddress.City));
    }
}