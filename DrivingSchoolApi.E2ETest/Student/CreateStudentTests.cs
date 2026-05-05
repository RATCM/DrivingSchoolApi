using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.E2ETest.Student;

public class CreateStudentTests : TestClass
{
    [Test]
    public async Task Create_Student_FailsWhenInvalidInviteId()
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
                    "Test1@email.com",
                    "12345678",
                    "test_password1"
                ));

        await AuthService.LoginInstructor(new LoginDto("test1@email.com", "test_password1"));
        
        await DrivingSchoolService.CreateInvite(createdSchool.Id);
        
        // Act
        var createStudentResponse = await StudentService.CreateStudent(
            new StudentRegistryDto(
                new NameDto("Test", "name"),
                "test2@email.com",
                "11111111",
                "test_password",
                Guid.NewGuid()));
        
        // Assert
        Assert.That(createStudentResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)); 
        // It should probably be a bad request
    }
    
    [Test]
    public async Task Create_Student_SucceedsWhenValidInviteId()
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
                    "Test1@email.com",
                    "12345678",
                    "test_password1"
                ));

        await AuthService.LoginInstructor(new LoginDto("test1@email.com", "test_password1"));
        
        var createInviteResponse = await DrivingSchoolService.CreateInvite(createdSchool.Id);

        var createdInvite = (await createInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>())!;
        
        // Act
        var createStudentResponse = await StudentService.CreateStudent(
            new StudentRegistryDto(
                new NameDto("Test", "name"),
                "test2@email.com",
                "11111111",
                "test_password",
                createdInvite.InviteId));
        
        // Assert
        Assert.That(createStudentResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created)); 
    }
}