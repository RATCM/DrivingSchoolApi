using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
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

        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var email = Email.Create("admin1@test.com");
        var password = "pw";
        var hashed = PasswordHash.Create("hashed-pw");

        guidService.NewGuid().Returns(adminId);
        passwordHasher.HashPassword(password).Returns(hashed);
        repo.Create(Arg.Any<Admin>()).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.CreateAdmin(email, password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(adminId));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(email));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(hashed));

        await repo.Received(1).Create(Arg.Is<Admin>(a =>
            a.Id.Value == adminId &&
            a.EmailAddress == email &&
            a.HashedPassword == hashed));

        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateAdmin_ReturnsFailure_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var email = Email.Create("admin2@test.com");
        var password = "pw";
        var hashed = PasswordHash.Create("hashed-pw");
        
        passwordHasher.HashPassword(password).Returns(hashed);
        repo.Create(Arg.Any<Admin>()).Returns(false);

        var sut = new AdminService(new GuidGeneratorService(), passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.CreateAdmin(email, password);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
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
        var adminId = AdminKey.Create(Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var email = Email.Create("admin3@test.com");
        var admin = Admin.Create(adminId, email, PasswordHash.Create("stored-hash"));

        repo.GetByEmail(email).Returns(admin);
        passwordHasher.VerifyHashedPassword(inputPassword, admin.HashedPassword).Returns(true);
        tokenGenerator.GenerateJwtAccessToken(adminId.Value, UserRole.Admin).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(adminId.Value, UserRole.Admin).Returns("refresh-token");

        var sut = new AdminService(new GuidGeneratorService(), passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.LoginAsAdmin(email, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.accessToken, Is.EqualTo("access-token"));
        Assert.That(result.Value.refreshToken, Is.EqualTo("refresh-token"));

        await repo.Received(1).GetByEmail(email);
        passwordHasher.Received(1).VerifyHashedPassword("pw", admin.HashedPassword);
    }

    [Test]
    public async Task LoginAsAdmin_ReturnsNotFound_OnFailure()
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
        Assert.That(result.Error, Is.Not.Null);
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
        var adminId = AdminKey.Create(Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var email = Email.Create("admin5@test.com");
        var admin = Admin.Create(adminId, email, PasswordHash.Create("stored-hash"));

        repo.GetByEmail(email).Returns(admin);
        passwordHasher.VerifyHashedPassword(inputPassword, admin.HashedPassword).Returns(false);
        tokenGenerator.GenerateJwtAccessToken(adminId.Value, UserRole.Admin).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(adminId.Value, UserRole.Admin).Returns("refresh-token");

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.LoginAsAdmin(email, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
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

        var adminId = AdminKey.Create(Guid.Parse("55555555-5555-5555-5555-555555555555"));
        var admin = Admin.Create(adminId, Email.Create("admin6@test.com"), PasswordHash.Create("hash"));

        repo.Get(adminId).Returns(admin);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAdminById(adminId);

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
        
        var sut = new AdminService(guidService,passwordHasher, tokenGenerator, repo);

        var adminId = AdminKey.Create(Guid.Parse("66666666-6666-6666-6666-666666666666"));

        // Act
        var result = await sut.GetAdminById(adminId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
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

        var first = Admin.Create(
            AdminKey.Create(Guid.Parse("77777777-7777-7777-7777-777777777777")),
            Email.Create("a@test.com"),
            PasswordHash.Create("hash-a"));

        var second = Admin.Create(
            AdminKey.Create(Guid.Parse("88888888-8888-8888-8888-888888888888")),
            Email.Create("b@test.com"),
            PasswordHash.Create("hash-b"));

        repo.GetAll().Returns([first, second]);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.GetAllAdmins();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(first));
        Assert.That(result.Value, Does.Contain(second));

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

        var adminId = AdminKey.Create(Guid.Parse("99999999-9999-9999-9999-999999999999"));
        var existing = Admin.Create(adminId, Email.Create("old@test.com"), PasswordHash.Create("old-hash"));
        var newEmail = Email.Create("new@test.com");
        var newPassword = "new-pw";
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(adminId).Returns(existing);
        passwordHasher.HashPassword(newPassword).Returns(newHash);
        repo.Update(Arg.Any<Admin>()).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.UpdateAdmin(adminId, newEmail, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo(adminId));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(newEmail));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(newHash));

        await repo.Received(1).Update(Arg.Is<Admin>(a =>
            a.Id == adminId &&
            a.EmailAddress == newEmail &&
            a.HashedPassword == newHash));

        await repo.Received(1).Save();
    }

    [Test]
    public async Task UpdateAdmin_ReturnsNotFound_OnFailure()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        var adminId = AdminKey.Create(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

        // Act
        var result = await sut.UpdateAdmin(adminId, Email.Create("new@test.com"), "new-pw");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
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

        var inputPassword = "new-pw";
        var adminId = AdminKey.Create(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        var existing = Admin.Create(adminId, Email.Create("old@test.com"), PasswordHash.Create("old-hash"));
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(adminId).Returns(existing);
        passwordHasher.HashPassword(inputPassword).Returns(newHash);
        repo.Update(Arg.Any<Admin>()).Returns(false);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.UpdateAdmin(adminId, Email.Create("new@test.com"), inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
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

        var adminId = AdminKey.Create(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        repo.Delete(adminId).Returns(true);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.DeleteAdmin(adminId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(adminId);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteAdmin_ReturnsNotFound_AndDoesNotSave_OnFailure()
    {
        // Arrange
        var repo = Substitute.For<IAdminRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Admin>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var adminId = AdminKey.Create(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        repo.Delete(adminId).Returns(false);

        var sut = new AdminService(guidService, passwordHasher, tokenGenerator, repo);

        // Act
        var result = await sut.DeleteAdmin(adminId);

        // Arrange
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.EqualTo("Admin could not be found"));

        await repo.Received(1).Delete(adminId);
        await repo.DidNotReceive().Save();
    }
}
