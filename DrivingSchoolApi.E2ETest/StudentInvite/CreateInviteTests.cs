using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.E2ETest.StudentInvite;

public class CreateInviteTests : TestClass
{
    [Test]
    public async Task Create_Invite_SucceedsWhenAuthorized()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        var createSchoolResponse = await 
            DrivingSchoolService
                .CreateDrivingSchool(
                    new DrivingSchoolRegistryDto(
                        "Test name",
                        "Test address",
                        "12345678",
                        "Test.com"));
        var createdSchool = (await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>())!;

        await
            InstructorService.CreateInstructor(
                new InstructorRegistryDto(
                    createdSchool.Id,
                    new NameDto("Test", "name"),
                    "test@email.com",
                    "12345678",
                    "test_password1"
                ));

        await AuthService.LoginInstructor(new LoginDto("Test@email.com", "test_password1"));
        
        // Act
        var createInviteResponse = await DrivingSchoolService.CreateInvite(createdSchool.Id);

        // Assert
        Assert.That(createInviteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}