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

public class DrivingSchoolServiceTests
{
    private IServiceCollection _services;
    
    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();

        _services
            .AddScoped<IDrivingSchoolService, DrivingSchoolService>()
            .AddScoped<IDrivingSchoolRepository>((_) => Substitute.For<IDrivingSchoolRepository>())
            .AddScoped<IGuidGeneratorService, GuidGeneratorService>();
    }
    
    [Test]
    public async Task CreateDrivingSchool_ReturnsSchool_WhenSuccess()
    {
        // Arrange
        _services.Replace(ServiceDescriptor.Scoped<IGuidGeneratorService>(
            (_) => Substitute.For<IGuidGeneratorService>()));
        
        await using var serviceProvider = _services.BuildServiceProvider();
        
        // We need to mock the guid generator to control the id that is being generated
        var guidGeneratorService = serviceProvider.GetRequiredService<IGuidGeneratorService>();
        guidGeneratorService.NewGuid()
            .Returns(Guid.Empty);

        var generatedDrivingSchool = DrivingSchool.GenerateRandom(id: Guid.Empty, seed: 0);

        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Create(generatedDrivingSchool)
            .Returns(true);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.CreateDrivingSchool(
            generatedDrivingSchool.DrivingSchoolName,
            generatedDrivingSchool.StreetAddress,
            generatedDrivingSchool.PhoneNumber,
            generatedDrivingSchool.WebAddress,
            generatedDrivingSchool.Packages.ToArray());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.True);
            Assert.That(drivingSchool.Value, Is.EqualTo(generatedDrivingSchool));
            Assert.DoesNotThrowAsync(async () => await mock.Received(1).Save());
        });
    }
    
    [Test]
    public async Task CreateDrivingSchool_ReturnsError_WhenFailed()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var generatedDrivingSchool = DrivingSchool.GenerateRandom(id: Guid.Empty, seed: 0);
        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Create(generatedDrivingSchool)
            .Returns(false);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.CreateDrivingSchool(
            generatedDrivingSchool.DrivingSchoolName,
            generatedDrivingSchool.StreetAddress,
            generatedDrivingSchool.PhoneNumber,
            generatedDrivingSchool.WebAddress,
            generatedDrivingSchool.Packages.ToArray());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.False);
            Assert.DoesNotThrowAsync(() => mock.DidNotReceive().Save());
        });
    }
    
    [Test]
    public async Task GetDrivingSchoolById_ReturnsDrivingSchool_WhenFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var generatedDrivingSchool = DrivingSchool.GenerateRandom(id: Guid.Empty, seed: 0);
        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(generatedDrivingSchool);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.That(drivingSchool.Value, Is.EqualTo(generatedDrivingSchool));
    }
    
    [Test]
    public async Task GetDrivingSchoolById_ReturnsError_WhenNotFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(null as DrivingSchool);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.False);
            Assert.That(drivingSchool.Error, Is.TypeOf<DrivingSchoolNotFoundException>());
        });
    }
    
    [Test]
    public async Task GetAllDrivingSchools_ReturnsDrivingSchools_WhenSuccess()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        DrivingSchool[] generatedDrivingSchools =
        [
            DrivingSchool.GenerateRandom(id: Guid.NewGuid(), seed: 0),
            DrivingSchool.GenerateRandom(id: Guid.NewGuid(), seed: 1),
            DrivingSchool.GenerateRandom(id: Guid.NewGuid(), seed: 2),
            DrivingSchool.GenerateRandom(id: Guid.NewGuid(), seed: 3)
        ];
        
        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .GetAll()
            .Returns(generatedDrivingSchools);
    
        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();
    
        // Act
        var drivingSchool = await sut.GetAllDrivingSchools();
    
        // Assert
        Assert.That(drivingSchool.Value?.ToArray(), Is.EqualTo(generatedDrivingSchools));
    }
    
    [Test]
    public async Task GetAllDrivingSchools_ReturnsError_WhenFailed()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();
    
        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(null as DrivingSchool);
    
        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();
    
        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(DrivingSchoolKey.Create(Guid.Empty));
    
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.False);
            Assert.That(drivingSchool.Error, Is.TypeOf<DrivingSchoolNotFoundException>());
        });
    }
    
    [Test]
    public async Task DeleteDrivingSchool_ReturnsSuccess_WhenFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Delete(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(true);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.DeleteDrivingSchool(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.True);
            Assert.DoesNotThrowAsync(async () => await mock.Received(1).Save());
        });
    }
    
    [Test]
    public async Task DeleteDrivingSchoolById_ReturnsError_WhenNotFound()
    {
        // Arrange
        await using var serviceProvider = _services.BuildServiceProvider();

        var mock = serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
        mock
            .Delete(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(false);

        var sut = serviceProvider.GetRequiredService<IDrivingSchoolService>();

        // Act
        var drivingSchool = await sut.DeleteDrivingSchool(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(drivingSchool.IsSuccess, Is.False);
            Assert.That(drivingSchool.Error, Is.TypeOf<DrivingSchoolNotFoundException>());
            Assert.DoesNotThrowAsync(async () => await mock.DidNotReceive().Save());
        });
    }
}