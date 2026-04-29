using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class DrivingLessonServiceTests
{
    private ServiceProvider _serviceProvider;

    public IDrivingLessonService GetSut()
    {
        return _serviceProvider.GetRequiredService<IDrivingLessonService>();
    }

    public IDrivingLessonRepository GetRepository()
    {
        return _serviceProvider.GetRequiredService<IDrivingLessonRepository>();
    }

    public IGuidGeneratorService GetGuidGenerator()
    {
        return _serviceProvider.GetRequiredService<IGuidGeneratorService>();
    }

    public IDrivingSchoolService GetDrivingSchoolService()
    {
        return _serviceProvider.GetRequiredService<IDrivingSchoolService>();
    }

    public IInstructorService GetInstructorService()
    {
        return _serviceProvider.GetRequiredService<IInstructorService>();
    }

    public IStudentService GetStudentService()
    {
        return _serviceProvider.GetRequiredService<IStudentService>();
    }
    
    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();

        collection
            .AddScoped<IDrivingLessonService, DrivingLessonService>()
            .AddScoped<IDrivingLessonRepository>(_ => Substitute.For<IDrivingLessonRepository>())
            .AddScoped<IGuidGeneratorService>(_ => Substitute.For<IGuidGeneratorService>())
            .AddScoped<IDrivingSchoolService>(_ => Substitute.For<IDrivingSchoolService>())
            .AddScoped<IInstructorService>(_ => Substitute.For<IInstructorService>())
            .AddScoped<IStudentService>(_ => Substitute.For<IStudentService>());
        
        _serviceProvider = collection.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public async Task CreateDrivingLesson_ReturnsDrivingLesson_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();
        var guidService = GetGuidGenerator();

        var lesson = DrivingLesson.CreateTestLesson();

        guidService.NewGuid().Returns(lesson.Id.Value);
        repo.Create(Arg.Any<DrivingLesson>()).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.CreateDrivingLesson(
            lesson.InstructorSignature.Blob, 
            lesson.StudentSignature.Blob, 
            lesson.SchoolId, lesson.Route, 
            lesson.Price, 
            lesson.InstructorId!, 
            lesson.StudentId!,
            lesson.CompletedObjective);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Id.Value, Is.EqualTo(lesson.Id.Value));
        Assert.That(result.Value.SchoolId, Is.EqualTo(lesson.SchoolId));
        Assert.That(result.Value.Route, Is.EqualTo(lesson.Route));
        Assert.That(result.Value.Price, Is.EqualTo(lesson.Price));
        Assert.That(result.Value.InstructorId, Is.EqualTo(lesson.InstructorId));
        Assert.That(result.Value.StudentId, Is.EqualTo(lesson.StudentId));

        await repo.Received(1).Create(Arg.Is<DrivingLesson>(l =>
            l.Id.Value == lesson.Id.Value &&
            l.SchoolId == lesson.SchoolId &&
            l.Route == lesson.Route &&
            l.Price == lesson.Price &&
            l.InstructorId == lesson.InstructorId &&
            l.StudentId == lesson.StudentId));

        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateDrivingLesson_ReturnsFailure_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = GetRepository();
        var guidService = GetGuidGenerator();
        
        var lesson = DrivingLesson.CreateTestLesson();

        guidService.NewGuid().Returns(lesson.Id.Value);
        repo.Create(Arg.Any<DrivingLesson>()).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.CreateDrivingLesson(
            lesson.InstructorSignature.Blob,
            lesson.StudentSignature.Blob,
            lesson.SchoolId,
            lesson.Route,
            lesson.Price,
            lesson.InstructorId!,
            lesson.StudentId!,
            lesson.CompletedObjective);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Message, Is.Not.Null);

        await repo.Received(1).Create(Arg.Any<DrivingLesson>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task GetDrivingLessonById_ReturnsDrivingLesson_WhenFound()
    {
        // Arrange
        var repo = GetRepository();

        var lesson = DrivingLesson.CreateTestLesson();

        repo.Get(lesson.Id).Returns(lesson);

        var sut = GetSut();

        // Act
        var result = await sut.GetDrivingLessonById(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(lesson));

        await repo.Received(1).Get(lesson.Id);
    }

    [Test]
    public async Task GetDrivingLessonById_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var repo = GetRepository();

        var lesson = DrivingLesson.CreateTestLesson();
        repo.Get(lesson.Id).Returns((DrivingLesson?)null);

        var sut = GetSut();

        // Act
        var result = await sut.GetDrivingLessonById(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.EqualTo("Error fetching driving lesson from DB."));

        await repo.Received(1).Get(lesson.Id);
    }

    [Test]
    public async Task GetAllDrivingLessonsFromSchool_ReturnsDrivingLessons_WhenRepoHasData()
    {
        // Arrange
        var repo = GetRepository();
        var drivingSchoolService = GetDrivingSchoolService();

        var drivingSchool = DrivingSchool.CreateTestSchool();
        var lesson1 = DrivingLesson.CreateTestLesson(schoolGuid: drivingSchool.Id.Value);
        var lesson2 = DrivingLesson.CreateTestLesson();

        drivingSchoolService.GetDrivingSchoolById(lesson1.SchoolId).Returns(drivingSchool);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromSchool(lesson1.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(1));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Not.Contain(lesson2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllDrivingLessonsFromSchool_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var repo = GetRepository();
        var drivingSchoolService = GetDrivingSchoolService();

        var drivingSchool = DrivingSchool.CreateTestSchool();
        var lesson1 = DrivingLesson.CreateTestLesson();
        var lesson2 = DrivingLesson.CreateTestLesson();

        drivingSchoolService.GetDrivingSchoolById(drivingSchool.Id).Returns(drivingSchool);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromSchool(drivingSchool.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllDrivingLessonsFromStudent_ReturnsDrivingLessons_WhenRepoHasData()
    {
        // Arrange
        var repo = GetRepository();
        var studentService = GetStudentService();

        var student = Student.CreateTestStudent();
        var lesson1 = DrivingLesson.CreateTestLesson(studentGuid: student.Id.Value);
        var lesson2 = DrivingLesson.CreateTestLesson();

        studentService.GetStudentById(student.Id).Returns(student);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromStudent(student.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(1));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Not.Contain(lesson2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllDrivingLessonsFromStudent_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var repo = GetRepository();
        var studentService = GetStudentService();
        
        var student = Student.CreateTestStudent();
        var lesson1 = DrivingLesson.CreateTestLesson();
        var lesson2 = DrivingLesson.CreateTestLesson();
        
        studentService.GetStudentById(student.Id).Returns(student);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromStudent(student.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllDrivingLessonsFromInstructor_ReturnsDrivingLessons_WhenRepoHasData()
    {
        // Arrange
        var repo = GetRepository();
        var instructorService = GetInstructorService();

        var instructor = Instructor.CreateTestInstructor();
        var lesson1 = DrivingLesson.CreateTestLesson(instructorGuid: instructor.Id.Value);
        var lesson2 = DrivingLesson.CreateTestLesson();

        instructorService.GetInstructorById(instructor.Id).Returns(instructor);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromInstructor(instructor.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(1));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Not.Contain(lesson2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllDrivingLessonsFromInstructor_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var repo = GetRepository();
        var instructorService = GetInstructorService();

        var instructor = Instructor.CreateTestInstructor();
        var lesson1 = DrivingLesson.CreateTestLesson();
        var lesson2 = DrivingLesson.CreateTestLesson();
        
        instructorService.GetInstructorById(instructor.Id).Returns(instructor);
        repo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllDrivingLessonsFromInstructor(instructor.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task DeleteDrivingLesson_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();

        var lesson = DrivingLesson.CreateTestLesson();
        
        repo.Delete(lesson.Id).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteDrivingLesson(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(lesson.Id);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteDrivingLesson_ReturnsNotFound_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = GetRepository();

        var lesson = DrivingLesson.CreateTestLesson();
        
        repo.Delete(lesson.Id).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteDrivingLesson(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Delete(lesson.Id);
        await repo.DidNotReceive().Save();
    }
}
