using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
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
            .Get(DrivingSchoolKey.Create(Guid.Empty))
            .Returns(DrivingSchool.Create(
                DrivingSchoolKey.Create(Guid.Empty),
                DrivingSchoolName.Create("Test School"),
                StreetAddress.Create("a", "b", "c", "d"),
                PhoneNumber.Create("1234"), 
                WebAddress.Create("url.com"),
                []));
        
        var sut = new DrivingSchoolService(new GuidGeneratorService(), mock);

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
}