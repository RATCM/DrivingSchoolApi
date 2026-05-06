using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.DrivingSchool;
using Newtonsoft.Json;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

public class GetDrivingSchoolTests : TestClass
{
    [Test]
    public async Task Get_DrivingSchool_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();

        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test name",
                "Test address",
                "12345678",
                "Test.com"));

        var createdSchool = JsonConvert.DeserializeObject<DrivingSchoolDto>(
            await createResponse.Content.ReadAsStringAsync());

        // Act
        var response = await DrivingSchoolService.GetDrivingSchool(createdSchool.Id);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task Get_DrivingSchool_ReturnsCorrectData()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test name",
                "Test address",
                "12345678",
                "Test.com"));

        var createdSchool = JsonConvert.DeserializeObject<DrivingSchoolDto>(
            await createResponse.Content.ReadAsStringAsync());

        // Act
        var response = await DrivingSchoolService.GetDrivingSchool(createdSchool.Id);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var school = JsonConvert.DeserializeObject<DrivingSchoolDto>(content);

        Assert.That(school, Is.Not.Null);
        Assert.That(school!.Id, Is.EqualTo(createdSchool.Id));
        Assert.That(school.Name, Is.EqualTo("Test name"));
        Assert.That(school.PhoneNumber, Is.EqualTo("12345678"));
        Assert.That(school.WebAddress, Is.EqualTo("Test.com"));

        // Nested object
        Assert.That(school.StreetAddress, Is.Not.Null);

        // Collections
        Assert.That(school.Packages, Is.Not.Null);
        Assert.That(school.Packages, Is.Empty);
        Assert.That(school.Students == null || school.Students.Count >= 0);
        Assert.That(school.Instructors == null || school.Instructors.Count >= 0);
    }
    
    [Test]
    public async Task Get_DrivingSchool_ReturnsNotFound_WhenIdDoesNotExist()
    {
        // Act
        var response = await DrivingSchoolService.GetDrivingSchool(Guid.NewGuid());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Get_All_DrivingSchools()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();

        var school1 = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test name 1",
                "Test address 1",
                "12345678",
                "Test1.com"));
        
        var school2 = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test name 2",
                "Test address 2",
                "12345678",
                "Test1.com"));
        
        var school3 = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test name 3",
                "Test address 3",
                "12345678",
                "Test1.com"));

        // Act
        var response = await DrivingSchoolService.GetAllDrivingSchools();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
