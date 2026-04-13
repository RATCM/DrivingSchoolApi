using System.Net;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.DrivingSchool;

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
                "Test address",
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
                        "Test address",
                        "12345678",
                        "Test.com"));

        // Assert
        Assert.That(createDrivingSchoolResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}