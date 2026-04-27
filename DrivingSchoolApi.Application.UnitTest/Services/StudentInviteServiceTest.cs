using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Exceptions.StudentInvite;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class StudentInviteServiceTest
{
    [Test]
    public async Task RedeemStudentInvite_ReturnsDrivingSchool_AndSaves_OnSuccess()
    {
        // Arrange
        var studentInviteRepo = Substitute.For<IStudentInviteRepository>();
        var drivingSchoolRepo = Substitute.For<IDrivingSchoolRepository>();
        var dateTimeProvider = Substitute.For<IDateTimeProviderService>();

        var now = new DateTime(2026, 01, 01, 12, 00, 00);
        
        var school = DrivingSchool.CreateTestSchool();
        var inviteId = StudentInviteKey.Create(Guid.NewGuid());
        var invite = StudentInvite.Create(inviteId, school.Id, now.AddHours(1));

        studentInviteRepo.Get(inviteId).Returns(invite);
        studentInviteRepo.Delete(inviteId).Returns(true);
        drivingSchoolRepo.Get(school.Id).Returns(school);
        dateTimeProvider.Now().Returns(now);

        var sut = new StudentInviteService(studentInviteRepo, drivingSchoolRepo, dateTimeProvider);

        // Act
        var result = await sut.RedeemStudentInvite(inviteId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo(school));

        await studentInviteRepo.Received(1).Get(inviteId);
        await studentInviteRepo.Received(1).Delete(inviteId);
        await drivingSchoolRepo.Received(1).Get(school.Id);
        await studentInviteRepo.Received(1).Save();
    }

    [Test]
    public async Task RedeemStudentInvite_ReturnsNotFound_WhenInviteDoesNotExist()
    {
        // Arrange
        var studentInviteRepo = Substitute.For<IStudentInviteRepository>();
        var drivingSchoolRepo = Substitute.For<IDrivingSchoolRepository>();
        var dateTimeProvider = Substitute.For<IDateTimeProviderService>();

        var inviteId = StudentInviteKey.Create(Guid.NewGuid());
        studentInviteRepo.Get(inviteId).Returns((StudentInvite?)null);

        var sut = new StudentInviteService(studentInviteRepo, drivingSchoolRepo, dateTimeProvider);
        
        // Act
        var result = await sut.RedeemStudentInvite(inviteId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentInviteNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await studentInviteRepo.Received(1).Get(inviteId);
        await studentInviteRepo.DidNotReceive().Delete(Arg.Any<StudentInviteKey>());
        await drivingSchoolRepo.DidNotReceive().Get(Arg.Any<DrivingSchoolKey>());
        await studentInviteRepo.DidNotReceive().Save();
    }

    [Test]
    public async Task RedeemStudentInvite_Throws_WhenDeleteFails()
    {
        // Arrange
        var studentInviteRepo = Substitute.For<IStudentInviteRepository>();
        var drivingSchoolRepo = Substitute.For<IDrivingSchoolRepository>();
        var dateTimeProvider = Substitute.For<IDateTimeProviderService>();
        
        var now  = new DateTime(2026, 01, 01, 12, 00, 00);
        
        var invite = StudentInvite.CreateTestInvite(now.AddHours(1));

        dateTimeProvider.Now().Returns(now);
        studentInviteRepo.Get(invite.Id).Returns(invite);
        studentInviteRepo.Delete(invite.Id).Returns(false);

        var sut = new StudentInviteService(studentInviteRepo, drivingSchoolRepo, dateTimeProvider);

        // Act
        var result = await sut.RedeemStudentInvite(invite.Id);
        
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);
    }

    [Test]
    public async Task RedeemStudentInvite_ReturnsExpired_WhenInviteIsExpired()
    {
        // Arrange
        var studentInviteRepo = Substitute.For<IStudentInviteRepository>();
        var drivingSchoolRepo = Substitute.For<IDrivingSchoolRepository>();
        var dateTimeProvider = Substitute.For<IDateTimeProviderService>();
        
        var now = new DateTime(2026, 02, 01, 10, 00, 00);
        var invite = StudentInvite.CreateTestInvite(now.AddMinutes(-1));

        studentInviteRepo.Get(invite.Id).Returns(invite);
        studentInviteRepo.Delete(invite.Id).Returns(true);
        dateTimeProvider.Now().Returns(now);

        var sut = new StudentInviteService(studentInviteRepo, drivingSchoolRepo, dateTimeProvider);

        // Act
        var result = await sut.RedeemStudentInvite(invite.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentInviteExpiredException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await studentInviteRepo.Received(1).Get(invite.Id);
        await studentInviteRepo.Received(1).Delete(invite.Id);
        await drivingSchoolRepo.DidNotReceive().Get(Arg.Any<DrivingSchoolKey>());
        await studentInviteRepo.DidNotReceive().Save();
    }

    [Test]
    public async Task RedeemStudentInvite_ReturnsNotFound_WhenDrivingSchoolDoesNotExist()
    {
        // Arrange
        var studentInviteRepo = Substitute.For<IStudentInviteRepository>();
        var drivingSchoolRepo = Substitute.For<IDrivingSchoolRepository>();
        var dateTimeProvider = Substitute.For<IDateTimeProviderService>();
        
        var now = new DateTime(2026, 03, 01, 08, 00, 00);
        var invite = StudentInvite.CreateTestInvite(now.AddHours(2));

        studentInviteRepo.Get(invite.Id).Returns(invite);
        studentInviteRepo.Delete(invite.Id).Returns(true);
        dateTimeProvider.Now().Returns(now);
        drivingSchoolRepo.Get(invite.DrivingSchoolId).Returns((DrivingSchool?)null);

        var sut = new StudentInviteService(studentInviteRepo, drivingSchoolRepo, dateTimeProvider);

        // Act
        var result = await sut.RedeemStudentInvite(invite.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<DrivingSchoolNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await studentInviteRepo.Received(1).Get(invite.Id);
        await studentInviteRepo.Received(1).Delete(invite.Id);
        await drivingSchoolRepo.Received(1).Get(invite.DrivingSchoolId);
        await studentInviteRepo.DidNotReceive().Save();
    }
}
