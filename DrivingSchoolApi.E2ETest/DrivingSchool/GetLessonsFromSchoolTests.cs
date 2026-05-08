using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingLesson;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.TheoryLesson;
using DrivingSchoolApi.Test.Extensions.Dtos;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

public class GetLessonsFromSchoolTests : TestClass
{
       
    [Test]
    public async Task GetDrivingSchoolTheoryLessons_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();

        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();

        var createInstructorResponse = await InstructorService.CreateInstructor(
            InstructorRegistryDto.CreateTestInstructor(createdSchool!.Id));
        var createdInstructor = await createInstructorResponse.Content.ReadFromJsonAsync<InstructorDto>();

        await AuthService.LoginInstructor(new LoginDto("instructor@test.com", "1234"));
        
        var createInviteResponse = await DrivingSchoolService.CreateInvite(createdSchool.Id);
        var createdInvite = await createInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();

        var createStudentResponse = await StudentService.CreateStudent(
            StudentRegistryDto.CreateTestStudent(createdInvite!.InviteId));
        var createdStudent = await createStudentResponse.Content.ReadFromJsonAsync<StudentDto>();
        
        var createTheoryLessonResponse = await InstructorService.CreateTheoryLesson(
            createdInstructor!.Id,
            TheoryLessonRegistryDto.CreateTestTheoryLesson(createdStudent!.Id));
        Assert.That(createTheoryLessonResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        
        // Act
        var getTheoryLessonsResponse = await DrivingSchoolService.GetDrivingSchoolTheoryLessons(createdSchool.Id);
        
        // Assert
        Assert.That(getTheoryLessonsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var theoryLessons = await getTheoryLessonsResponse.Content.ReadFromJsonAsync<List<TheoryLessonDto>>();
        Assert.That(theoryLessons, Has.Count.EqualTo(1));
        Assert.That(theoryLessons[0].StudentId, Is.EqualTo(createdStudent.Id));
    }
    
    [Test]
    public async Task GetDrivingSchoolDrivingLessons_Succeeds()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();

        var createSchoolResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createSchoolResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();

        var createInstructorResponse = await InstructorService.CreateInstructor(
            InstructorRegistryDto.CreateTestInstructor(createdSchool!.Id));
        var createdInstructor = await createInstructorResponse.Content.ReadFromJsonAsync<InstructorDto>();

        await AuthService.LoginInstructor(new LoginDto("instructor@test.com", "1234"));
        
        var createInviteResponse = await DrivingSchoolService.CreateInvite(createdSchool.Id);
        var createdInvite = await createInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();

        var createStudentResponse = await StudentService.CreateStudent(
            StudentRegistryDto.CreateTestStudent(createdInvite!.InviteId));
        var createdStudent = await createStudentResponse.Content.ReadFromJsonAsync<StudentDto>();
        
        var createDrivingLessonResponse = await InstructorService.CreateDrivingLesson(
            createdInstructor!.Id,
            DrivingLessonRegistryDto.CreateTestDrivingLesson(createdStudent!.Id, createdSchool.Id));

        
        // Act
        var getDrivingLessonsResponse = await DrivingSchoolService.GetDrivingSchoolDrivingLessons(createdSchool.Id);
        
        // Assert
        Assert.That(createDrivingLessonResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(getDrivingLessonsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var drivingLessons = await getDrivingLessonsResponse.Content.ReadFromJsonAsync<List<DrivingLessonDto>>();
        Assert.That(drivingLessons, Has.Count.EqualTo(1));
        Assert.That(drivingLessons[0].StudentId, Is.EqualTo(createdStudent.Id));
    }
}