using System.Net;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

public class CreateDrivingSchoolTests : TestClass
{
    [Test]
    public async Task Create_DrivingSchool_FailsWhenNotAuthenticated()
    {
        // Act
        var createDrivingSchoolResponse = await 
            DrivingSchoolService
            .CreateDrivingSchool(
                new DrivingSchoolRegistryDto(
                "Test name",
                new StreetAddressDto("2800","Lyngby","Hovedstaden","Nybrovej"),
                "12345678",
                "Test.com"));
        
        // Assert
        Assert.That(createDrivingSchoolResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Create_DrivingSchool_SucceedsWhenAuthorized()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        // Act
        var createDrivingSchoolResponse = await 
            DrivingSchoolService
                .CreateDrivingSchool(
                    new DrivingSchoolRegistryDto(
                        "Test name",
                        new StreetAddressDto("2800","Lyngby","Hovedstaden","Nybrovej"),
                        "12345678",
                        "Test.com"));

        // Assert
        Assert.That(createDrivingSchoolResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}