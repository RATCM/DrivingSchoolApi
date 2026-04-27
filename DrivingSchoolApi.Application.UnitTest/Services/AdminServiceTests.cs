using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Exceptions.Admin;
using DrivingSchoolApi.Application.Exceptions.Common;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class AdminServiceTests
{
    [Test]
    public async Task CreateAdmin_ReturnsAdmin_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var password = "pw";
        var admin = Admin.CreateTestAdmin(password: "testPassword");

        guidService.NewGuid().Returns(admin.Id.Value);
        passwordHasher.HashPassword(password).Returns(admin.HashedPassword);
        repo.Create(Arg.Any<Admin>()).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.CreateAdmin(admin.EmailAddress, password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(admin.Id.Value));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(admin.EmailAddress));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(admin.HashedPassword));

        await repo.Received(1).Create(Arg.Any<Admin>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateAdmin_ReturnsFailure_AndDoesNotSave_WhenCreationFails()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var password = "pw";
        var admin = Admin.CreateTestAdmin();
        
        passwordHasher.HashPassword(password).Returns(admin.HashedPassword);
        repo.Create(Arg.Any<Admin>()).Returns(false);

        var sut = new AdminService(new GuidGeneratorService(), passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.CreateAdmin(admin.EmailAddress, password);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Create(Arg.Any<Admin>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task LoginAsAdmin_ReturnsTokens_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var inputPassword = "pw";
        var admin = Admin.CreateTestAdmin();

        repo.GetByEmail(admin.EmailAddress).Returns(admin);
        passwordHasher.VerifyHashedPassword(inputPassword, admin.HashedPassword).Returns(true);
        tokenGenerator.GenerateJwtAccessToken(admin.Id.Value, UserRole.Admin).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(admin.Id.Value, UserRole.Admin).Returns("refresh-token");

        var sut = new AdminService(new GuidGeneratorService(), passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.LoginAsAdmin(admin.EmailAddress, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.accessToken, Is.EqualTo("access-token"));
        Assert.That(result.Value.refreshToken, Is.EqualTo("refresh-token"));

        await repo.Received(1).GetByEmail(admin.EmailAddress);
        passwordHasher.Received(1).VerifyHashedPassword("pw", admin.HashedPassword);
    }

    [Test]
    public async Task LoginAsAdmin_ReturnsNotFound_WhenAdminDoesntExist()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var email = Email.Create("admin4@test.com");
        repo.GetByEmail(email).Returns((Admin?)null);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.LoginAsAdmin(email, "pw");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<AdminNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);
        
        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task LoginAsAdmin_ReturnsInvalidLogin_WhenPasswordIsWrong()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var inputPassword = "incorrect";
        var admin = Admin.CreateTestAdmin();

        repo.GetByEmail(admin.EmailAddress).Returns(admin);
        passwordHasher.VerifyHashedPassword(inputPassword, admin.HashedPassword).Returns(false);
        tokenGenerator.GenerateJwtAccessToken(admin.Id.Value, UserRole.Admin).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(admin.Id.Value, UserRole.Admin).Returns("refresh-token");

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.LoginAsAdmin(admin.EmailAddress, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InvalidLoginRequestException>());
        Assert.That(result.Error!.Message, Is.Not.Null);
        
        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task GetAdminById_ReturnsAdmin_WhenFound()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var admin = Admin.CreateTestAdmin();

        repo.Get(admin.Id).Returns(admin);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAdminById(admin.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Id, Is.EqualTo(admin.Id));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(admin.EmailAddress));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(admin.HashedPassword));
        
        await repo.Received(1).Get(Arg.Any<AdminKey>());
    }

    [Test]
    public async Task GetAdminById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var admin = Admin.CreateTestAdmin();
        
        var sut = new AdminService(guidService,passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAdminById(admin.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<AdminNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);
        
        await repo.Received(1).Get(Arg.Any<AdminKey>());
    }

    [Test]
    public async Task GetAllAdmins_WhenPopulated_ReturnsAllAdmins()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var admin1 = Admin.CreateTestAdmin();
        var admin2 = Admin.CreateTestAdmin();

        repo.GetAll().Returns([admin1, admin2]);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAllAdmins();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(admin1));
        Assert.That(result.Value, Does.Contain(admin2));

        await repo.Received(1).GetAll();
    }
    
    [Test]
    public async Task GetAllAdmins_ReturnsEmpty_WhenEmpty()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>(); ;

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAllAdmins();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }
    
    [Test]
    public async Task UpdateAdmin_ReturnsUpdatedAdmin_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var admin = Admin.CreateTestAdmin();
        var newEmail = Email.Create("new@test.com");
        var newPassword = "new-pw";
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(admin.Id).Returns(admin);
        passwordHasher.HashPassword(newPassword).Returns(newHash);
        repo.Update(Arg.Any<Admin>()).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.UpdateAdmin(admin.Id, newEmail, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo(admin.Id));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(newEmail));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(newHash));

        await repo.Received(1).Update(Arg.Any<Admin>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task UpdateAdmin_ReturnsNotFound_WhenAdminDoesntExist()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);
        
        var admin = Admin.CreateTestAdmin();

        // Act
        var result = await sut.UpdateAdmin(admin.Id, Email.Create("new@test.com"), "new-pw");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<AdminNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.DidNotReceive().Update(Arg.Any<Admin>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateAdmin_ReturnsFailure_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var admin = Admin.CreateTestAdmin();
        var newEmail = Email.Create("newAdmin@test.com");
        var newPassword = "new-pw";
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(admin.Id).Returns(admin);
        passwordHasher.HashPassword(newPassword).Returns(newHash);
        repo.Update(Arg.Any<Admin>()).Returns(false);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.UpdateAdmin(admin.Id, newEmail, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Update(Arg.Any<Admin>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task DeleteAdmin_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var admin = Admin.CreateTestAdmin();
        repo.Delete(admin.Id).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.DeleteAdmin(admin.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(admin.Id);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteAdmin_ReturnsNotFound_AndDoesNotSave_WhenAdminDoesntExist()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var admin = Admin.CreateTestAdmin();
        repo.Delete(admin.Id).Returns(false);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.DeleteAdmin(admin.Id);

        // Arrange
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<AdminNotFoundException>());
        Assert.That(result.Error!.Message, Is.EqualTo("Admin could not be found"));

        await repo.Received(1).Delete(admin.Id);
        await repo.DidNotReceive().Save();
    }
}
