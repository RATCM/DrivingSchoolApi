using DrivingSchoolApi.Application.Exceptions.DrivingLesson;
using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class DrivingLessonServiceTests
{
    private IServiceCollection _services;
    
    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();

        _services
            .AddScoped<IDrivingLessonService, DrivingLessonService>()
            .AddScoped<IDrivingLessonRepository>((_) => Substitute.For<IDrivingLessonRepository>())
            .AddScoped<IDrivingSchoolRepository>((_) => Substitute.For<IDrivingSchoolRepository>())
            .AddScoped<IStudentRepository>((_) => Substitute.For<IStudentRepository>())
            .AddScoped<IInstructorRepository>((_) => Substitute.For<IInstructorRepository>())
            .AddScoped<IGuidGeneratorService, GuidGeneratorService>();
    }
    
    [Test]
    public async Task CreateDrivingLesson_ReturnsDrivingLesson_WhenSuccess()
    {
        // Arrange
        _services.Replace(ServiceDescriptor.Scoped<IGuidGeneratorService>(
            (_) => Substitute.For<IGuidGeneratorService>()));

        await using var serviceProvider = _services.BuildServiceProvider();

        // We need to mock the guid generator to control the id that is being generated
        var guidGeneratorService = serviceProvider.GetRequiredService<IGuidGeneratorService>();
        guidGeneratorService.NewGuid()
            .Returns(Guid.Empty);

        var generated = DrivingLesson.GenerateRandom(
            InstructorKey.Create(Guid.Empty),
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            id: Guid.Empty,
            seed: 0);
        var mock = serviceProvider.GetRequiredService<IDrivingLessonRepository>();
        mock
            .Create(generated)
            .Returns(true);

        var sut = serviceProvider.GetRequiredService<IDrivingLessonService>();

        // Act
        var drivingLesson = await sut.CreateDrivingLesson(
            generated.SchoolId,
            generated.Route,
            generated.Price,
            generated.InstructorId,
            generated.StudentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingLesson.IsSuccess, Is.True);
            Assert.That(drivingLesson.Value, Is.EqualTo(generated));
            Assert.DoesNotThrowAsync(async () => await mock.Received(1).Save());
        });
    }
    
    [Test]
    public async Task CreateDrivingLesson_ReturnsError_WhenFailed()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var generated = DrivingLesson.GenerateRandom(
            InstructorKey.Create(Guid.Empty),
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            id: Guid.Empty,
            seed: 0);
        var mock = serviceProvider.GetRequiredService<IDrivingLessonRepository>();
        mock
            .Create(generated)
            .Returns(false);

        var sut = serviceProvider.GetRequiredService<IDrivingLessonService>();

        // Act
        var drivingLesson = await sut.CreateDrivingLesson(
            generated.SchoolId,
            generated.Route,
            generated.Price,
            generated.InstructorId,
            generated.StudentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingLesson.IsSuccess, Is.False);
            Assert.DoesNotThrowAsync(async () => await mock.DidNotReceive().Save());
        });
    }
    
    [Test]
    public async Task GetDrivingLessonById_ReturnsDrivingLesson_WhenFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var generated = DrivingLesson.GenerateRandom(
            InstructorKey.Create(Guid.Empty),
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            id: Guid.Empty,
            seed: 0
        );
        var mock = serviceProvider.GetRequiredService<IDrivingLessonRepository>();
        mock
            .Get(DrivingLessonKey.Create(Guid.Empty))
            .Returns(generated);

        var sut = serviceProvider.GetRequiredService<IDrivingLessonService>();

        // Act
        var drivingLesson = await sut.GetDrivingLessonById(DrivingLessonKey.Create(Guid.Empty));

        // Assert
        Assert.That(drivingLesson.Value, Is.EqualTo(generated));
    }
    
    [Test]
    public async Task GetAllDrivingLessonsFromSchool_ReturnsDrivingLessons_WhenFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();
        
        var generated = DrivingLesson.GenerateRandomArray(
            5,
            [
                (DrivingSchoolKey.Create(Guid.Empty), 
                    [StudentKey.Create(Guid.NewGuid()), StudentKey.Create(Guid.NewGuid())],
                    [InstructorKey.Create(Guid.NewGuid()), InstructorKey.Create(Guid.NewGuid())])
            ],
            seed: 0
        );
        var drivingLessonRepository = serviceProvider.GetRequiredService<IDrivingLessonRepository>();
        var drivingSchoolRepository = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        
        drivingLessonRepository
            .GetAll()
            .Returns(generated);
        drivingSchoolRepository
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(DrivingSchool.GenerateRandom(Guid.Empty));

        var sut = serviceProvider.GetRequiredService<IDrivingLessonService>();

        // Act
        var drivingLesson = await sut.GetAllDrivingLessonsFromSchool(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.That(drivingLesson.Value, Is.EqualTo(generated));
    }
    
    [Test]
    public async Task GetDrivingLessonById_ReturnsError_WhenNotFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var mock = serviceProvider.GetRequiredService<IDrivingLessonRepository>();
        mock
            .Get(DrivingLessonKey.Create(Guid.Empty))
            .Returns(null as DrivingLesson);

        var sut = serviceProvider.GetRequiredService<IDrivingLessonService>();

        // Act
        var drivingLesson = await sut.GetDrivingLessonById(DrivingLessonKey.Create(Guid.Empty));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingLesson.IsSuccess, Is.False);
            Assert.That(drivingLesson.Error, Is.TypeOf<DrivingLessonNotFoundException>());
        });
    }
}