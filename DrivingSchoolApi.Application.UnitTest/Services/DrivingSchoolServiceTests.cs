using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class DrivingSchoolServiceTests
{
    private ServiceProvider _serviceProvider;
    
    private IDrivingSchoolService GetSut()
    {
        return _serviceProvider.GetRequiredService<IDrivingSchoolService>();
    }

    private IDrivingSchoolRepository GetRepository()
    {
        return _serviceProvider.GetRequiredService<IDrivingSchoolRepository>();
    }

    private IGuidGeneratorService GetGuidGenerator()
    {
        return _serviceProvider.GetRequiredService<IGuidGeneratorService>();
    }

    private IDateTimeProviderService GetDateTimeProvider()
    {
        return _serviceProvider.GetRequiredService<IDateTimeProviderService>();
    }
    
    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();

        collection
            .AddScoped<IDrivingSchoolService, DrivingSchoolService>()
            .AddScoped<IDrivingSchoolRepository>(_ => Substitute.For<IDrivingSchoolRepository>())
            .AddScoped<IGuidGeneratorService>(_ => Substitute.For<IGuidGeneratorService>())
            .AddScoped<IDateTimeProviderService>(_ => Substitute.For<IDateTimeProviderService>());

        
        _serviceProvider = collection.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    
    [Test]
    public async Task CreateDrivingSchool_ReturnsDrivingSchool_AndSaves_OnSucces()
    {
        // Arrange
        var repo = GetRepository();
        repo.Create(Arg.Any<DrivingSchool>()).Returns(true);
        
        var expectedId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        GetGuidGenerator().NewGuid().Returns(expectedId);

        var sut = GetSut();
    
        var name = DrivingSchoolName.Create("Test School");
        var address = StreetAddress.Create("12345", "City", "Region", "Main St 1");
        var phone = PhoneNumber.Create("1234");
        var web = WebAddress.Create("test.com");
        var packages = Array.Empty<Package>();
    
        // Act
        var result = await sut.CreateDrivingSchool(name, address, phone, web, packages);
    
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(expectedId));
        Assert.That(result.Value.DrivingSchoolName, Is.EqualTo(name));
        Assert.That(result.Value.StreetAddress, Is.EqualTo(address));
        Assert.That(result.Value.PhoneNumber, Is.EqualTo(phone));
        Assert.That(result.Value.WebAddress, Is.EqualTo(web));
    
        // acts as assertions
        await repo.Received(1).Create(Arg.Is<DrivingSchool>(d =>
            d.Id.Value == expectedId &&
            d.DrivingSchoolName == name &&
            d.StreetAddress == address &&
            d.PhoneNumber == phone &&
            d.WebAddress == web));
    
        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateDrivingSchool_ReturnsFailure_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = GetRepository();
        repo.Create(Arg.Any<DrivingSchool>()).Returns(false);

        var sut = GetSut();
    
        // Act
        var result = await sut.CreateDrivingSchool(
            DrivingSchoolName.Create("Test School"),
            StreetAddress.Create("12345", "City", "Region", "Main St 1"),
            PhoneNumber.Create("1234"),
            WebAddress.Create("test.com"),
            Array.Empty<Package>());
    
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.EqualTo("Unable to create driving school"));
    
        await repo.Received(1).Create(Arg.Any<DrivingSchool>());
        await repo.DidNotReceive().Save();
    }
    
    [Test]
    public async Task GetDrivingSchoolById_ReturnsDrivingSchool_WhenFound()
    {
        // Arrange
        var mock = GetRepository();
        mock
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(DrivingSchool.Create(
                DrivingSchoolKey.Create(Guid.Empty),
                DrivingSchoolName.Create("Test School"),
                StreetAddress.Create("a", "b", "c", "d"),
                PhoneNumber.Create("1234"), 
                WebAddress.Create("url.com"),
                []));

        var sut = GetSut();

        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(DrivingSchoolKey.Create(Guid.Empty));

        // Assert
        Assert.That(drivingSchool.Value, Is.EqualTo(DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Empty),
            DrivingSchoolName.Create("Test School"),
            StreetAddress.Create("a", "b", "c", "d"),
            PhoneNumber.Create("1234"), 
            WebAddress.Create("url.com"),
            [])));
    }

    [Test]
    public async Task GetDrivingSchoolById_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var sut = GetSut();
        
        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(DrivingSchoolKey.Create(Guid.Empty));
        
        // Assert
        Assert.That(drivingSchool.Value, Is.Null);
    }

[Test]
    public async Task GetAllDrivingSchools_ReturnsDrivingSchools_WhenRepoHasData()
    {
        // Arrange
        var repo = GetRepository();
    
        var first = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DrivingSchoolName.Create("School 1"),
            StreetAddress.Create("1000", "CityA", "RegionA", "Street 1"),
            PhoneNumber.Create("1111"),
            WebAddress.Create("school1.com"),
            []);
    
        var second = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.Parse("22222222-2222-2222-2222-222222222222")),
            DrivingSchoolName.Create("School 2"),
            StreetAddress.Create("2000", "CityB", "RegionB", "Street 2"),
            PhoneNumber.Create("2222"),
            WebAddress.Create("school2.com"),
            []);
    
        repo.GetAll().Returns([first, second]);

        var sut = GetSut();
        
        // Act
        var result = await sut.GetAllDrivingSchools();
    
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(first));
        Assert.That(result.Value, Does.Contain(second));
    
        await repo.Received(1).GetAll();
    }
    
    [Test]
    public async Task GetAllDrivingSchools_ReturnsEmpty_WhenEmpty()
    {
        var repo = GetRepository();
        repo.GetAll().Returns(Array.Empty<DrivingSchool>());

        var sut = GetSut();
    
        var result = await sut.GetAllDrivingSchools();
    
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!, Is.Empty);
    
        await repo.Received(1).GetAll();
    }
    
    [Test]
    public async Task CreateStudentInvite_ReturnsInvite_AndAddsItToDrivingSchool_WhenDrivingSchoolExists()
    {
        // Arrange
        var repo = GetRepository();
        var guidService = GetGuidGenerator();
        var dateTimeProvider = GetDateTimeProvider();
    
        var drivingSchoolId = DrivingSchoolKey.Create(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        var inviteId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var timeValid = TimeSpan.FromHours(2);
    
        var school = DrivingSchool.Create(
            drivingSchoolId,
            DrivingSchoolName.Create("Test School"),
            StreetAddress.Create("12345", "City", "Region", "Main St 1"),
            PhoneNumber.Create("1234"),
            WebAddress.Create("test.com"),
            Array.Empty<Package>());

        var now = new DateTime(2000, 1, 1).ToUniversalTime();
        repo.Get(drivingSchoolId).Returns(school);
        guidService.NewGuid().Returns(inviteId);
        dateTimeProvider.Now().Returns(now);

        var sut = GetSut();

        var before = now;
    
        // Act
        var result = await sut.CreateStudentInvite(drivingSchoolId, timeValid);

        var after = now.AddMinutes(60);
    
        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(inviteId));
        Assert.That(result.Value.DrivingSchoolId, Is.EqualTo(drivingSchoolId));
    
        // Allow a tiny window because service uses DateTime.Now directly
        Assert.That(result.Value.ExpirationDateTime, Is.GreaterThanOrEqualTo(before.Add(timeValid)));
        Assert.That(result.Value.ExpirationDateTime, Is.LessThanOrEqualTo(after.Add(timeValid)));
    
        Assert.That(school.StudentInvites.Count, Is.EqualTo(1));
        Assert.That(school.StudentInvites[0].Id.Value, Is.EqualTo(inviteId));
    
        await repo.Received(1).Get(drivingSchoolId);
    }
    
    [Test]
    public async Task CreateStudentInvite_ReturnsNotFound_WhenDrivingSchoolDoesNotExist()
    {
        // Arrange
        var repo = GetRepository();
        var sut = GetSut();
    
        var drivingSchoolId = DrivingSchoolKey.Create(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
    
        // Act
        var result = await sut.CreateStudentInvite(drivingSchoolId, TimeSpan.FromHours(1));
    
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.EqualTo("Driving school not found"));
    
        await repo.Received(1).Get(drivingSchoolId);
    }
    
    [Test]
    public async Task DeleteDrivingSchool_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();
        repo.Delete(Arg.Any<DrivingSchoolKey>()).Returns(true);

        var sut = GetSut();

        var drivingSchoolId = DrivingSchoolKey.Create(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));

        // Act
        var result = await sut.DeleteDrivingSchool(drivingSchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(drivingSchoolId);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteDrivingSchool_ReturnsNotFound_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = GetRepository();
        repo.Delete(Arg.Any<DrivingSchoolKey>()).Returns(false);

        var sut = GetSut();

        var drivingSchoolId = DrivingSchoolKey.Create(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

        // Act
        var result = await sut.DeleteDrivingSchool(drivingSchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Delete(drivingSchoolId);
        await repo.DidNotReceive().Save();
    }
}
