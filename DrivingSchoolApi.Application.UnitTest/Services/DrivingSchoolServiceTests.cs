using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class DrivingSchoolServiceTests
{
    [Test]
    public async Task GetDrivingSchoolById_ReturnsDrivingSchool_WhenFound()
    {
        // Arrange
        var mock = Substitute.For<IDrivingSchoolRepository>();
        mock
            .Get(Guid.Empty)
            .Returns(new DrivingSchool(Guid.Empty,
                new DrivingSchoolName("Test School"),
                new StreetAddress("a", "b", "c", "d"),
                new PhoneNumber("1234"), 
                new WebAddress("url.com"),
                new Money(100, "DKK")));
        
        var sut = new DrivingSchoolService(mock);

        // Act
        var drivingSchool = await sut.GetDrivingSchoolById(Guid.Empty);

        // Assert
        Assert.That(drivingSchool, Is.EqualTo(new DrivingSchool(Guid.Empty,
            new DrivingSchoolName("Test School"),
            new StreetAddress("a", "b", "c", "d"),
            new PhoneNumber("1234"), 
            new WebAddress("url.com"),
            new Money(100, "DKK"))));
    }
}