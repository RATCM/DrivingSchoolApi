using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class StudentInviteRepositoryTests : TestClass
{
    [Test]
    public async Task GetAll_Returns_AllInvites()
    {
        // Arrange
        var schoolA = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 1"),
            StreetAddress.Create("1234", "test city 1", "test region 1", "test address 1"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool1.com"),
            []);
        var schoolB = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 2"),
            StreetAddress.Create("5678", "test city 2", "test region 2", "test address 2"),
            PhoneNumber.Create("22222222"),
            WebAddress.Create("testSchool2.com"),
            []);

        var invite1 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 1));
        var invite2 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 2));
        var invite3 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 3));
        var invite4 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolB.Id,
            new DateTime(2000, 1, 4));
        
        schoolA.AddStudentInvite(invite1);
        schoolA.AddStudentInvite(invite2);
        schoolA.AddStudentInvite(invite3);
        schoolB.AddStudentInvite(invite4);

        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();
        
        // Act
        var inviteRepository = GetStudentInviteRepository();
        var invites = await inviteRepository.GetAll();
        
        // Assert
        Assert.That(invites, Is.EquivalentTo([invite1, invite2, invite3, invite4]));
    }
    
    [Test]
    public async Task Get_Return_AllInvite()
    {
        // Arrange
        var schoolA = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 1"),
            StreetAddress.Create("1234", "test city 1", "test region 1", "test address 1"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool1.com"),
            []);
        var schoolB = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 2"),
            StreetAddress.Create("5678", "test city 2", "test region 2", "test address 2"),
            PhoneNumber.Create("22222222"),
            WebAddress.Create("testSchool2.com"),
            []);

        var invite1 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 1));
        var invite2 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 2));
        var invite3 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolA.Id,
            new DateTime(2000, 1, 3));
        var invite4 = StudentInvite.Create(
            StudentInviteKey.Create(Guid.NewGuid()),
            schoolB.Id,
            new DateTime(2000, 1, 4));
        
        schoolA.AddStudentInvite(invite1);
        schoolA.AddStudentInvite(invite2);
        schoolA.AddStudentInvite(invite3);
        schoolB.AddStudentInvite(invite4);

        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();
        
        // Act
        var inviteRepository = GetStudentInviteRepository();
        var invite = await inviteRepository.Get(invite1.Id);
        
        // Assert
        Assert.That(invite, Is.EqualTo(invite1));
    }

}