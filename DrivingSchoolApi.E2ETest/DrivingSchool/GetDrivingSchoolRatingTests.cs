using System.Net;
using System.Net.Http.Json;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.CompletedCourse;
using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.TheoryLesson;
using DrivingSchoolApi.DTOs.ValueObject;
using DrivingSchoolApi.Test.Extensions.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace DrivingSchoolApi.E2ETest.DrivingSchool;

public class GetDrivingSchoolRatingTests : TestClass
{
    

    [Test]
    public async Task GetDrivingSchoolRating_ReturnsNotFound_WhenSchoolDoesNotExist()
    {
        // Act
        var response = await DrivingSchoolService.GetDrivingSchoolRating(Guid.NewGuid());

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetDrivingSchoolRating_ReturnsCorrectData_WithNoCompletedCourses()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            new DrivingSchoolRegistryDto(
                "Test School",
                new StreetAddressDto(
                    "4040",
                    "Jyllinge",
                    "Hovedstaden",
                    "Test address 1"),
                "12345678",
                "Test.com"));

        var createdSchool = JsonConvert.DeserializeObject<DrivingSchoolDto>(
            await createResponse.Content.ReadAsStringAsync());

        // Act
        var response = await DrivingSchoolService.GetDrivingSchoolRating(createdSchool!.Id);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var content = await response.Content.ReadAsStringAsync();
        var rating = JsonConvert.DeserializeObject<DrivingSchoolRatingDto>(content);

        Assert.That(rating, Is.Not.Null);
        Assert.That(rating!.PassRate, Is.EqualTo(0));
        Assert.That(rating.FailRate, Is.EqualTo(0));
        Assert.That(rating.QuitRate, Is.EqualTo(0));
        Assert.That(rating.AveragePrice.Amount, Is.EqualTo(0));
    }

    [Test]
    public async Task GetDrivingSchoolRating_AllStudentsFinished()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        var schoolId = createdSchool!.Id;

        var instructor = await InstructorService.CreateInstructor(
            new InstructorRegistryDto(
                schoolId, 
                new NameDto("lars", "larsen"), 
                "test1@email.com", 
                "11223344", 
                "1234"));
        var instructorBody = await instructor.Content.ReadFromJsonAsync<InstructorDto>();

        await AuthService.LoginInstructor(new LoginDto("test1@email.com", "1234"));
        
        var studentInviteResponse = await DrivingSchoolService.CreateInvite(schoolId);
        var studentInviteBody = await studentInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();
        var studentInviteId = studentInviteBody!.InviteId;
        var createStudent = await StudentService.CreateStudent(
            new StudentRegistryDto(
                new NameDto("lars", "larsen"), 
                "test2@email.com", 
                "11223344", 
                "1234", 
                studentInviteId));
        var studentBody = await createStudent.Content.ReadFromJsonAsync<StudentDto>();
        var theoryLesson = await InstructorService.CreateTheoryLesson(
            instructorBody!.Id,
            TheoryLessonRegistryDto.CreateTestTheoryLesson(studentBody!.Id));

        // 1 finished
        await StudentService.CompleteCourse(
            studentBody.Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 31),
                "Finished"));
        
        // Act
        var response = await DrivingSchoolService.GetDrivingSchoolRating(schoolId);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var rating = await response.Content.ReadFromJsonAsync<DrivingSchoolRatingDto>();

        Assert.That(rating, Is.Not.Null);
        Assert.That(rating!.PassRate, Is.EqualTo(1).Within(0.01));
        Assert.That(rating.FailRate, Is.EqualTo(0).Within(0.01));
        Assert.That(rating.QuitRate, Is.EqualTo(0).Within(0.01));
        Assert.That(rating.AveragePrice.Amount, Is.EqualTo(500).Within(0.01));
    }

    [Test]
    public async Task GetDrivingSchoolRating_CalculatesCorrectAveragePrice_OnlyForFinishedCourses()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        var schoolId = createdSchool!.Id;

        var instructor = await InstructorService.CreateInstructor(
            new InstructorRegistryDto(
                schoolId, 
                new NameDto("lars", "larsen"), 
                "test1@email.com", 
                "11223344", 
                "1234"));
        var instructorBody = await instructor.Content.ReadFromJsonAsync<InstructorDto>();

        await AuthService.LoginInstructor(new LoginDto("test1@email.com", "1234"));
        

        // Create 3 students: 1 finished, 1 failed, 1 quit
        var students = new List<StudentDto>();
        for (int i = 0; i < 3; i++)
        {
            var studentInviteResponse = await DrivingSchoolService.CreateInvite(schoolId);
            var studentInviteBody = await studentInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();
            var studentInviteId = studentInviteBody!.InviteId;

            var createStudent = await StudentService.CreateStudent(
                new StudentRegistryDto(
                    new NameDto("student", $"larsen{i}"), 
                    $"student{i}@email.com", 
                    "11223344", 
                    "1234", 
                    studentInviteId));
            var studentBody = await createStudent.Content.ReadFromJsonAsync<StudentDto>();
            students.Add(studentBody!);
        }

        await InstructorService.CreateTheoryLesson(
            instructorBody!.Id,
            TheoryLessonRegistryDto.CreateTestTheoryLesson(students[0].Id));

        // 1 finished (cost 1000), 1 failed (cost 5000), 1 quit (cost 10000)
        // Average should only be 1000 (only finished courses)
        await StudentService.CompleteCourse(
            students[0].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 12),
                "Finished"));

        await StudentService.CompleteCourse(
            students[1].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 12),
                "Failed"));

        await StudentService.CompleteCourse(
            students[2].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 12),
                "Quit"));
        
        // Act
        var response = await DrivingSchoolService.GetDrivingSchoolRating(schoolId);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var rating = await response.Content.ReadFromJsonAsync<DrivingSchoolRatingDto>();

        Assert.That(rating, Is.Not.Null);
        Assert.That(rating!.AveragePrice.Amount, Is.EqualTo(500).Within(0.01));
        Assert.That(rating.PassRate, Is.EqualTo(0.333).Within(0.01)); // 1/3
        Assert.That(rating.FailRate, Is.EqualTo(0.333).Within(0.01)); // 1/3
        Assert.That(rating.QuitRate, Is.EqualTo(0.333).Within(0.01)); // 1/3
    
    }

    [Test]
    public async Task GetDrivingSchoolRating_CalculatesCorrectRates_WithMultipleCompletedCourses()
    {
        // Arrange
        await AuthService.LoginAsDefaultAdmin();
        var createResponse = await DrivingSchoolService.CreateDrivingSchool(
            DrivingSchoolRegistryDto.CreateTestSchool());
        var createdSchool = await createResponse.Content.ReadFromJsonAsync<DrivingSchoolDto>();
        var schoolId = createdSchool!.Id;

        var instructor = await InstructorService.CreateInstructor(
            new InstructorRegistryDto(
                schoolId, 
                new NameDto("lars", "larsen"), 
                "test1@email.com", 
                "11223344", 
                "1234"));
        var instructorBody = await instructor.Content.ReadFromJsonAsync<InstructorDto>();

        await AuthService.LoginInstructor(new LoginDto("test1@email.com", "1234"));
        


        // Create 4 students: 2 finished, 1 failed, 1 quit
        var students = new List<StudentDto>();
        for (int i = 0; i < 4; i++)
        {
            var studentInviteResponse = await DrivingSchoolService.CreateInvite(schoolId);
            var studentInviteBody = await studentInviteResponse.Content.ReadFromJsonAsync<StudentInviteDto>();
            var studentInviteId = studentInviteBody!.InviteId;
            
            var createStudent = await StudentService.CreateStudent(
                new StudentRegistryDto(
                    new NameDto("student", $"larsen{i}"), 
                    $"student{i}@email.com", 
                    "11223344", 
                    "1234", 
                    studentInviteId));
            var studentBody = await createStudent.Content.ReadFromJsonAsync<StudentDto>();
            students.Add(studentBody!);
        }

        // Create theory lessons for all students
        foreach (var student in students)
        {
            await InstructorService.CreateTheoryLesson(
                instructorBody!.Id,
                TheoryLessonRegistryDto.CreateTestTheoryLesson(student.Id));
        }

        // 2 finished
        await StudentService.CompleteCourse(
            students[0].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 31),
                "Finished"));

        await StudentService.CompleteCourse(
            students[1].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 31),
                "Finished"));

        // 1 failed
        await StudentService.CompleteCourse(
            students[2].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 31),
                "Failed"));

        // 1 quit
        await StudentService.CompleteCourse(
            students[3].Id, 
            new CompletedCourseRegistryDto(
                new DateTime(1999, 12, 31),
                "Quit"));
        
        // Act
        var response = await DrivingSchoolService.GetDrivingSchoolRating(schoolId);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var rating = await response.Content.ReadFromJsonAsync<DrivingSchoolRatingDto>();

        Assert.That(rating, Is.Not.Null);
        Assert.That(rating.PassRate, Is.EqualTo(0.5).Within(0.01)); // 2/4
        Assert.That(rating.FailRate, Is.EqualTo(0.25).Within(0.01)); // 1/4
        Assert.That(rating.QuitRate, Is.EqualTo(0.25).Within(0.01)); // 1/4
        Assert.That(rating.AveragePrice.Amount, Is.EqualTo(500).Within(0.01)); // Average of 2 finished courses
    }
}