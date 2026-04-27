using DrivingSchoolApi.Application.Exceptions.Instructor;
using DrivingSchoolApi.Application.Exceptions.TheoryLesson;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class TheoryLessonServiceTests
{
    private ServiceProvider _serviceProvider;

    private ITheoryLessonService GetSut()
    {
        return _serviceProvider.GetRequiredService<ITheoryLessonService>();
    }

    private ITheoryLessonRepository GetRepository()
    {
        return _serviceProvider.GetRequiredService<ITheoryLessonRepository>();
    }

    private IGuidGeneratorService GetGuidGenerator()
    {
        return _serviceProvider.GetRequiredService<IGuidGeneratorService>();
    }

    private IInstructorService GetInstructorService()
    {
        return _serviceProvider.GetRequiredService<IInstructorService>();
    }
    
    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();
        collection
            .AddScoped<ITheoryLessonService, TheoryLessonService>()
            .AddScoped<ITheoryLessonRepository>(_ => Substitute.For<ITheoryLessonRepository>())
            .AddScoped<IGuidGeneratorService>(_ => Substitute.For<IGuidGeneratorService>())
            .AddScoped<IInstructorService>(_ => Substitute.For<IInstructorService>());

        _serviceProvider = collection.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public async Task CreateTheoryLesson_ReturnsTheoryLesson_AndSaves_OnSuccess()
    {
        // Arrange
        var lessonRepo = GetRepository();
        var instructorService = GetInstructorService();
        var guidService = GetGuidGenerator();
        
        var lesson = TheoryLesson.CreateTestTheoryLesson();
        var instructor = Instructor.CreateTestInstructor(schoolGuid: lesson.SchoolId.Value);

        guidService.NewGuid().Returns(lesson.Id.Value);
        instructorService.GetInstructorById(lesson.InstructorId!).Returns(instructor);
        lessonRepo.Create(lesson).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.CreateTheoryLesson(
            lesson.InstructorSignature.Blob,
            lesson.StudentSignature.Blob,
            lesson.InstructorId!,
            lesson.LessonDateTime,
            lesson.Price,
            lesson.StudentId!);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo(lesson.Id));
        Assert.That(result.Value.SchoolId, Is.EqualTo(instructor.SchoolId));
        Assert.That(result.Value.LessonDateTime, Is.EqualTo(lesson.LessonDateTime));
        Assert.That(result.Value.Price, Is.EqualTo(lesson.Price));
        Assert.That(result.Value.InstructorId, Is.EqualTo(lesson.InstructorId));
        Assert.That(result.Value.StudentId, Is.EqualTo(lesson.StudentId));

        await instructorService.Received(1).GetInstructorById(lesson.InstructorId!);
        await lessonRepo.Received(1).Create(Arg.Any<TheoryLesson>());
        await lessonRepo.Received(1).Save();
    }

    [Test]
    public async Task CreateTheoryLesson_ReturnsNotFound_WhenInstructorDoesNotExist()
    {
        // Arrange
        var lessonRepo = GetRepository();
        var instructorService = GetInstructorService();
        
        var lesson = TheoryLesson.CreateTestTheoryLesson();
        
        instructorService.GetInstructorById(lesson.InstructorId!).Returns(new InstructorNotFoundException(""));

        var sut = GetSut();
        
        // Act
        var result = await sut.CreateTheoryLesson(
            lesson.InstructorSignature.Blob,
            lesson.StudentSignature.Blob,
            lesson.InstructorId!,
            lesson.LessonDateTime,
            lesson.Price,
            lesson.StudentId!);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await instructorService.Received(1).GetInstructorById(lesson.InstructorId!);
        await lessonRepo.DidNotReceive().Create(Arg.Any<TheoryLesson>());
        await lessonRepo.DidNotReceive().Save();
    }

    [Test]
    public async Task CreateTheoryLesson_ReturnsFailure_AndDoesNotSave_OnRepositoryFailure()
    {
        // Arrange
        var lessonRepo = GetRepository();
        var instructorService = GetInstructorService();
        var guidService = GetGuidGenerator();
        
        var lesson = TheoryLesson.CreateTestTheoryLesson();
        var instructor = Instructor.CreateTestInstructor(schoolGuid: lesson.SchoolId.Value);

        guidService.NewGuid().Returns(lesson.Id.Value);
        instructorService.GetInstructorById(lesson.InstructorId!).Returns(instructor);
        lessonRepo.Create(Arg.Any<TheoryLesson>()).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.CreateTheoryLesson(
            lesson.InstructorSignature.Blob,
            lesson.StudentSignature.Blob,
            lesson.InstructorId!,
            lesson.LessonDateTime,
            lesson.Price,
            lesson.StudentId!);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await instructorService.Received(1).GetInstructorById(lesson.InstructorId!);
        await lessonRepo.Received(1).Create(Arg.Any<TheoryLesson>());
        await lessonRepo.DidNotReceive().Save();
    }

    [Test]
    public async Task GetTheoryLessonById_ReturnsTheoryLesson_WhenFound()
    {
        // Arrange
        var lessonRepo = GetRepository();

        var lesson = TheoryLesson.CreateTestTheoryLesson();
        lessonRepo.Get(lesson.Id).Returns(lesson);

        var sut = GetSut();

        // Act
        var result = await sut.GetTheoryLessonById(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(lesson));

        await lessonRepo.Received(1).Get(lesson.Id);
    }

    [Test]
    public async Task GetTheoryLessonById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var lessonRepo = GetRepository();

        var lesson = TheoryLesson.CreateTestTheoryLesson(DateTime.UtcNow);
        lessonRepo.Get(lesson.Id).Returns((TheoryLesson?)null);

        var sut = GetSut();

        // Act
        var result = await sut.GetTheoryLessonById(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<TheoryLessonNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await lessonRepo.Received(1).Get(lesson.Id);
    }

    [Test]
    public async Task GetAllTheoryLessonsFromSchool_ReturnsTheoryLessons_WhenRepoHasData()
    {
        // Arrange
        var lessonRepo = GetRepository();
        
        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson(schoolId: lesson1.SchoolId.Value);
        var lesson3 = TheoryLesson.CreateTestTheoryLesson();

        lessonRepo.GetAll().Returns([lesson1, lesson2, lesson3]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromSchool(lesson1.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Contain(lesson2));
        Assert.That(result.Value, Does.Not.Contain(lesson3));

        await lessonRepo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllTheoryLessonsFromSchool_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var lessonRepo = GetRepository();
        
        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson();
        var otherSchoolId = DrivingSchoolKey.Create(Guid.NewGuid());

        lessonRepo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromSchool(otherSchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await lessonRepo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllTheoryLessonsFromStudent_ReturnsTheoryLessons_WhenRepoHasData()
    {
        // Arrange
        var lessonRepo = GetRepository();
        
        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson(studentId: lesson1.StudentId!.Value);
        var lesson3 = TheoryLesson.CreateTestTheoryLesson();

        lessonRepo.GetAll().Returns([lesson1, lesson2, lesson3]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromStudent(lesson1.StudentId!);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Contain(lesson2));
        Assert.That(result.Value, Does.Not.Contain(lesson3));

        await lessonRepo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllTheoryLessonsFromStudent_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var lessonRepo = GetRepository();
        
        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson();
        var otherStudentId = StudentKey.Create(Guid.NewGuid());

        lessonRepo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromStudent(otherStudentId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await lessonRepo.Received(1).GetAll();
    }
    
    [Test]
    public async Task GetAllTheoryLessonsFromInstructor_ReturnsTheoryLessons_WhenRepoHasData()
    {
        // Arrange
        var lessonRepo = GetRepository();
        
        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson(instructorId: lesson1.InstructorId!.Value);
        var lesson3 = TheoryLesson.CreateTestTheoryLesson();

        lessonRepo.GetAll().Returns([lesson1, lesson2, lesson3]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromInstructor(lesson1.InstructorId!);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(lesson1));
        Assert.That(result.Value, Does.Contain(lesson2));
        Assert.That(result.Value, Does.Not.Contain(lesson3));

        await lessonRepo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllTheoryLessonsFromInstructor_ReturnsEmpty_WhenNoMatchingLessons()
    {
        // Arrange
        var lessonRepo = GetRepository();

        var lesson1 = TheoryLesson.CreateTestTheoryLesson();
        var lesson2 = TheoryLesson.CreateTestTheoryLesson();
        var otherInstructorId = InstructorKey.Create(Guid.NewGuid());

        lessonRepo.GetAll().Returns([lesson1, lesson2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllTheoryLessonsFromInstructor(otherInstructorId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await lessonRepo.Received(1).GetAll();
    }

    [Test]
    public async Task DeleteTheoryLesson_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var lessonRepo = GetRepository();

        var lesson = TheoryLesson.CreateTestTheoryLesson();
        lessonRepo.Delete(lesson.Id).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteTheoryLesson(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await lessonRepo.Received(1).Delete(lesson.Id);
        await lessonRepo.Received(1).Save();
    }

    [Test]
    public async Task DeleteTheoryLesson_ReturnsNotFound_AndDoesNotSave_WhenNotFound()
    {
        // Arrange
        var lessonRepo = GetRepository();

        var lesson = TheoryLesson.CreateTestTheoryLesson();
        lessonRepo.Delete(lesson.Id).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteTheoryLesson(lesson.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<TheoryLessonNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await lessonRepo.Received(1).Delete(lesson.Id);
        await lessonRepo.DidNotReceive().Save();
    }
}