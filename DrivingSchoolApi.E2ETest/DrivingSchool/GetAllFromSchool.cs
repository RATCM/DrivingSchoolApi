using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.ValueObject;
using DrivingSchoolApi.Test.Extensions.Dtos;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

//Tests GetAllStudentsFromSchool and GetAllInstructorsFromSchool

public class GetAllFromSchool : TestClass
{
    [Test]
    public async Task GetAllStudentsFromSchool_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();

        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();

        await InstructorService.CreateInstructor(
                new InstructorRegistryDto(
                    createdSchool!.Id,
                    new NameDto("Test", "Instructor"),
                    "instructor@test.com",
                    "87654321",
                    "password123"
                ));

        await AuthService.LoginInstructor(new LoginDto("instructor@test.com", "password123"));
        
        var createInviteResponse = await DrivingSchoolService.CreateInvite(createdSchool.Id);
        var createdInvite = await createInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();
        
        await StudentService.CreateStudent(
            new StudentRegistryDto(
                new NameDto("Test", "Student"),
                "student@test.com",
                "11111111",
                "password456",
                createdInvite!.InviteId));
        
        // Act
        var getStudentsResponse = await DrivingSchoolService.GetAllStudentsFromSchool(createdSchool.Id);
        
        // Assert
        Assert.That(getStudentsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var students = await getStudentsResponse.Content.ReadFromJsonAsync<List<StudentDto>>();
        Assert.That(students, Has.Count.EqualTo(1));
        Assert.That(students[0].EmailAddress, Is.EqualTo("student@test.com"));
    }
    
    [Test]
    public async Task GetAllInstructorsFromSchool_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        
        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        var schoolId = createdSchool!.Id;

        await InstructorService.CreateInstructor(
                new InstructorRegistryDto(
                    schoolId,
                    new NameDto("Test", "Instructor"),
                    "instructor@test.com",
                    "87654321",
                    "password123"
                ));

        //await AuthService.LoginInstructor(new LoginDto("instructor@test.com", "password123"));

        // Act
        var getInstructorsResponse = await DrivingSchoolService.GetAllInstructorsFromSchool(schoolId);

        // Assert
        Assert.That(getInstructorsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var instructors = await getInstructorsResponse.Content.ReadFromJsonAsync<List<InstructorDto>>();
        Assert.That(instructors, Has.Count.EqualTo(1));
        Assert.That(instructors[0].EmailAddress, Is.EqualTo("instructor@test.com"));
    }
}