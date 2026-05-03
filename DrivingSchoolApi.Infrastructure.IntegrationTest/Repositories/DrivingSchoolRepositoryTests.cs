using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class DrivingSchoolRepositoryTests : TestClass
{
    [Test]
    public async Task Update_ReturnsTrue_WhenDrivingSchoolExists()
    {
        // Arrange
        var existingSchool = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name"),
            StreetAddress.Create("1234", "test city", "test region", "test address"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool.com"),
            [
                Package.Create("Package 1 title", "Package 1 description", Money.Create(20000, "DKK")), 
                Package.Create("Package 2 title", "Package 2 description", Money.Create(10000, "DKK"))
            ]);
        
        var newSchool = DrivingSchool.Create(
            existingSchool.Id,
            DrivingSchoolName.Create("new name"),
            StreetAddress.Create("5678", "new city", "new region", "new address"),
            PhoneNumber.Create("22222222"),
            WebAddress.Create("newSchool.com"),
            [
                Package.Create("Package 1 title", "new Package 1 description", Money.Create(10000, "USD")), 
                Package.Create("Package 3 title", "Package 3 description", Money.Create(30000, "DKK"))
            ]);

        var schoolRepository = GetDrivingSchoolRepository();

        await schoolRepository.Create(existingSchool);
        await schoolRepository.Save();
        
        // Act
        var recv = await schoolRepository.Update(newSchool);
        await schoolRepository.Save();
        
        var recvSchool = await schoolRepository.Get(newSchool.Id);
        
        // Assert
        Assert.That(recv, Is.True);
        Assert.That(recvSchool, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(recvSchool.Id, Is.EqualTo(newSchool.Id));
            Assert.That(recvSchool.DrivingSchoolName, Is.EqualTo(newSchool.DrivingSchoolName));
            Assert.That(recvSchool.PhoneNumber, Is.EqualTo(newSchool.PhoneNumber));
            Assert.That(recvSchool.WebAddress, Is.EqualTo(newSchool.WebAddress));
            Assert.That(recvSchool.StreetAddress, Is.EqualTo(newSchool.StreetAddress));
            Assert.That(recvSchool.StudentInvites.ToHashSet(), Is.EqualTo(newSchool.StudentInvites.ToHashSet()));
            Assert.That(recvSchool.Packages.ToHashSet(), Is.EqualTo(newSchool.Packages.ToHashSet()));
        }
        );
    }
    
    [Test]
    public async Task Update_ReturnsFalse_WhenDrivingSchoolDoesntExist()
    {
        // Arrange
        var newSchool = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name"),
            StreetAddress.Create("1234", "test city", "test region", "test address"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool.com"),
            [
                Package.Create("Package 1 title", "Package 1 description", Money.Create(20000, "DKK")), 
                Package.Create("Package 2 title", "Package 2 description", Money.Create(10000, "DKK"))
            ]);

        var schoolRepository = GetDrivingSchoolRepository();
        
        // Act
        var recv = await schoolRepository.Update(newSchool);
        await schoolRepository.Save();
        
        // Assert
        Assert.That(recv, Is.False);
    }
}