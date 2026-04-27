using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Exceptions.Common;
using DrivingSchoolApi.Application.Exceptions.Instructor;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class InstructorServiceTest
{
    [Test]
    public async Task LoginAsInstructor_ReturnsTokens_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var inputPassword = "password123";
        var instructor = Instructor.CreateTestInstructor();

        repo.GetByEmail(instructor.EmailAddress).Returns(instructor);
        passwordHasher.VerifyHashedPassword(inputPassword, instructor.HashedPassword).Returns(true);
        tokenGenerator.GenerateJwtAccessToken(instructor.Id.Value, UserRole.Instructor).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(instructor.Id.Value, UserRole.Instructor).Returns("refresh-token");

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.LoginAsInstructor(instructor.EmailAddress.Address, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.AccessToken, Is.EqualTo("access-token"));
        Assert.That(result.Value.RefreshToken, Is.EqualTo("refresh-token"));

        await repo.Received(1).GetByEmail(instructor.EmailAddress);
        passwordHasher.Received(1).VerifyHashedPassword(inputPassword, instructor.HashedPassword);
    }

    [Test]
    public async Task LoginAsInstructor_ReturnsNotFound_WhenInstructorDoesntExist()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var email = Email.Create("instructor@test.com");
        
        repo.GetByEmail(email).Returns((Instructor?)null);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.LoginAsInstructor(email.Address, "pw");
        
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task LoginAsInstructor_ReturnsInvalidLogin_WhenPasswordIsWrong()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var inputPassword = "incorrect";
        var instructor = Instructor.CreateTestInstructor();

        repo.GetByEmail(instructor.EmailAddress).Returns(instructor);
        passwordHasher.VerifyHashedPassword(inputPassword, instructor.HashedPassword).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.LoginAsInstructor(instructor.EmailAddress.Address, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InvalidLoginRequestException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task CreateInstructor_ReturnsInstructor_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var password = "pw";
        var instructor = Instructor.CreateTestInstructor();

        guidService.NewGuid().Returns(instructor.Id.Value);
        passwordHasher.HashPassword(password).Returns(instructor.HashedPassword);
        repo.Create(Arg.Any<Instructor>()).Returns(true);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.CreateInstructor(instructor.InstructorName, instructor.EmailAddress, password, instructor.PhoneNumber, instructor.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(instructor.Id.Value));
        Assert.That(result.Value.InstructorName, Is.EqualTo(instructor.InstructorName));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(instructor.EmailAddress));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(instructor.HashedPassword));
        Assert.That(result.Value.PhoneNumber, Is.EqualTo(instructor.PhoneNumber));
        Assert.That(result.Value.SchoolId, Is.EqualTo(instructor.SchoolId));

        await repo.Received(1).Create(Arg.Any<Instructor>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateInstructor_ReturnsFailure_AndDoesNotSave_WhenCreationFails()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var password = "pw";
        var instructor = Instructor.CreateTestInstructor();

        passwordHasher.HashPassword(password).Returns(instructor.HashedPassword);
        repo.Create(Arg.Any<Instructor>()).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.CreateInstructor(instructor.InstructorName, instructor.EmailAddress, password, instructor.PhoneNumber, instructor.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Create(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task GetAllInstructors_ReturnsAllInstructors_WhenPopulated()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor1 = Instructor.CreateTestInstructor();
        var instructor2 = Instructor.CreateTestInstructor();

        repo.GetAll().Returns([instructor1, instructor2]);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetAllInstructors();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(instructor1));
        Assert.That(result.Value, Does.Contain(instructor2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllInstructors_ReturnsEmpty_WhenEmpty()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        repo.GetAll().Returns(Array.Empty<Instructor>());

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetAllInstructors();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetInstructorById_ReturnsInstructor_WhenFound()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor = Instructor.CreateTestInstructor();

        repo.Get(instructor.Id).Returns(instructor);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetInstructorById(instructor.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Id, Is.EqualTo(instructor.Id));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(instructor.EmailAddress));
        Assert.That(result.Value.InstructorName, Is.EqualTo(instructor.InstructorName));

        await repo.Received(1).Get(Arg.Any<InstructorKey>());
    }

    [Test]
    public async Task GetInstructorById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructorId = InstructorKey.Create(Guid.Parse("55555555-5555-5555-5555-555555555555"));

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetInstructorById(instructorId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(Arg.Any<InstructorKey>());
    }

    [Test]
    public async Task GetAllInstructorsFromSchool_ReturnsInstructors_WhenRepoHasData()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor1 = Instructor.CreateTestInstructor();

        var instructor2 = Instructor.CreateTestInstructor();

        repo.GetAll().Returns([instructor1, instructor2]);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetAllInstructorsFromSchool(instructor1.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(1));
        Assert.That(result.Value, Does.Contain(instructor1));
        Assert.That(result.Value, Does.Not.Contain(instructor2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllInstructorsFromSchool_ReturnsEmpty_WhenNoMatchingInstructors()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor = Instructor.CreateTestInstructor();
        var otherSchoolId = DrivingSchoolKey.Create(Guid.NewGuid());
        
        repo.GetAll().Returns([instructor]);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetAllInstructorsFromSchool(otherSchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetInstructorDrivingSchoolId_ReturnsDrivingSchoolId_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructor = Instructor.CreateTestInstructor();

        repo.Get(instructor.Id).Returns(instructor);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetInstructorDrivingSchoolId(instructor.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(instructor.SchoolId));

        await repo.Received(1).Get(instructor.Id);
    }

    [Test]
    public async Task GetInstructorDrivingSchoolId_ReturnsNotFound_WhenInstructorNotExists()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var randomInstructorId = InstructorKey.Create(Guid.NewGuid());

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.GetInstructorDrivingSchoolId(randomInstructorId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(randomInstructorId);
    }

    [Test]
    public async Task UpdateInstructor_ReturnsUpdatedInstructor_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructor = Instructor.CreateTestInstructor();

        var newName = Name.Create("New", "Name");
        var newEmail = Email.Create("new@test.com");
        var newPhoneNumber = PhoneNumber.Create("2222222222");

        repo.Get(instructor.Id).Returns(instructor);
        repo.Update(Arg.Any<Instructor>()).Returns(true);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructor(instructor.Id, newName, newEmail, newPhoneNumber);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id, Is.EqualTo(instructor.Id));
        Assert.That(result.Value.InstructorName, Is.EqualTo(newName));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(newEmail));
        Assert.That(result.Value.PhoneNumber, Is.EqualTo(newPhoneNumber));
        Assert.That(result.Value.SchoolId, Is.EqualTo(instructor.SchoolId));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(instructor.HashedPassword));

        await repo.Received(1).Update(Arg.Any<Instructor>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task UpdateInstructor_ReturnsNotFound_WhenInstructorNotExists()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructor = Instructor.CreateTestInstructor();

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructor(
            instructor.Id,
            instructor.InstructorName,
            instructor.EmailAddress,
            instructor.PhoneNumber);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.DidNotReceive().Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateInstructor_ReturnsFailure_AndDoesNotSave_OnRepositoryFailure()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor = Instructor.CreateTestInstructor();

        repo.Get(instructor.Id).Returns(instructor);
        repo.Update(Arg.Any<Instructor>()).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructor(
            instructor.Id,
            instructor.InstructorName,
            instructor.EmailAddress,
            instructor.PhoneNumber);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateInstructorPassword_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructor = Instructor.CreateTestInstructor();
        var oldPassword = "oldPassword123";
        var newPassword = "newPassword456";
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(instructor.Id).Returns(instructor);
        passwordHasher.VerifyHashedPassword(oldPassword, instructor.HashedPassword).Returns(true);
        passwordHasher.HashPassword(newPassword).Returns(newHash);
        repo.Update(Arg.Any<Instructor>()).Returns(true);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructorPassword(instructor.Id, oldPassword, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        await repo.Received(1).Get(instructor.Id);
        passwordHasher.Received(1).VerifyHashedPassword(oldPassword, instructor.HashedPassword);
        passwordHasher.Received(1).HashPassword(newPassword);
        await repo.Received(1).Update(Arg.Any<Instructor>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task UpdateInstructorPassword_ReturnsFailure_WhenNewPasswordSameAsOld()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructorId = InstructorKey.Create(Guid.NewGuid());
        var samePassword = "samePassword";

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructorPassword(instructorId, samePassword, samePassword);
        
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InvalidPasswordException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.DidNotReceive().Get(Arg.Any<InstructorKey>());
        await repo.DidNotReceive().Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateInstructorPassword_ReturnsNotFound_WhenInstructorDoesntExists()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructorId = InstructorKey.Create(Guid.NewGuid());

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructorPassword(instructorId, "oldPassword", "newPassword");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(instructorId);
        await repo.DidNotReceive().Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateInstructorPassword_ReturnsFailure_WhenOldPasswordIncorrect()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor = Instructor.CreateTestInstructor();
        var oldPassword = "wrongPassword";
        var newPassword = "newPassword456";

        repo.Get(instructor.Id).Returns(instructor);
        passwordHasher.VerifyHashedPassword(oldPassword, instructor.HashedPassword).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructorPassword(instructor.Id, oldPassword, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InvalidPasswordException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(instructor.Id);
        passwordHasher.Received(1).VerifyHashedPassword(oldPassword, instructor.HashedPassword);
        await repo.DidNotReceive().Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task UpdateInstructorPassword_ReturnsFailure_AndDoesNotSave_OnRepositoryFailure()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();
        
        var instructor = Instructor.CreateTestInstructor();
        var oldPassword = "oldPassword123";
        var newPassword = "newPassword456";
        var newHash = PasswordHash.Create("new-hash");

        repo.Get(instructor.Id).Returns(instructor);
        passwordHasher.VerifyHashedPassword(oldPassword, instructor.HashedPassword).Returns(true);
        passwordHasher.HashPassword(newPassword).Returns(newHash);
        repo.Update(Arg.Any<Instructor>()).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.UpdateInstructorPassword(instructor.Id, oldPassword, newPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Update(Arg.Any<Instructor>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task DeleteInstructor_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructorId = InstructorKey.Create(Guid.NewGuid());
        repo.Delete(instructorId).Returns(true);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.DeleteInstructor(instructorId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(instructorId);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteInstructor_ReturnsNotFound_AndDoesNotSave_WhenInstructorNotFound()
    {
        // Arrange
        var repo = Substitute.For<IInstructorRepository>();
        var guidService = Substitute.For<IGuidGeneratorService>();
        var passwordHasher = Substitute.For<IPasswordHasher<Instructor>>();
        var tokenGenerator = Substitute.For<ITokenGeneratorService>();

        var instructorId = InstructorKey.Create(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        repo.Delete(instructorId).Returns(false);

        var sut = new InstructorService(guidService, repo, tokenGenerator, passwordHasher);

        // Act
        var result = await sut.DeleteInstructor(instructorId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InstructorNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Delete(instructorId);
        await repo.DidNotReceive().Save();
    }
}
