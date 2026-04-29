using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Exceptions.Common;
using DrivingSchoolApi.Application.Exceptions.Student;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class StudentServiceTests
{
    private ServiceProvider _serviceProvider;

    private IStudentService GetSut()
    {
        return _serviceProvider.GetRequiredService<IStudentService>();
    }

    private IStudentRepository GetRepository()
    {
        return _serviceProvider.GetRequiredService<IStudentRepository>();
    }

    private IGuidGeneratorService GetGuidGenerator()
    {
        return _serviceProvider.GetRequiredService<IGuidGeneratorService>();
    }

    private ITokenGeneratorService GetTokenGenerator()
    {
        return _serviceProvider.GetRequiredService<ITokenGeneratorService>();
    }

    private IPasswordHasher<Student> GetPasswordHasher()
    {
        return _serviceProvider.GetRequiredService<IPasswordHasher<Student>>();
    }
    
    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();
        collection
            .AddScoped<IStudentService, StudentService>()
            .AddScoped<IStudentRepository>(_ => Substitute.For<IStudentRepository>())
            .AddScoped<IGuidGeneratorService>(_ => Substitute.For<IGuidGeneratorService>())
            .AddScoped<ITokenGeneratorService>(_ => Substitute.For<ITokenGeneratorService>())
            .AddScoped<IPasswordHasher<Student>>(_ => Substitute.For<IPasswordHasher<Student>>());
        
        _serviceProvider = collection.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public async Task LoginAsStudent_ReturnsTokens_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();
        var tokenGenerator = GetTokenGenerator();
        var passwordHasher = GetPasswordHasher();

        var inputPassword = "password123";
        var student = Student.CreateTestStudent();

        repo.GetByEmail(student.EmailAddress).Returns(student);
        passwordHasher.VerifyHashedPassword(inputPassword, student.HashedPassword).Returns(true);
        tokenGenerator.GenerateJwtAccessToken(student.Id.Value, UserRole.Student).Returns("access-token");
        tokenGenerator.GenerateJwtRefreshToken(student.Id.Value, UserRole.Student).Returns("refresh-token");

        var sut = GetSut();

        // Act
        var result = await sut.LoginAsStudent(student.EmailAddress.Address, inputPassword);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.AccessToken, Is.EqualTo("access-token"));
        Assert.That(result.Value.RefreshToken, Is.EqualTo("refresh-token"));

        await repo.Received(1).GetByEmail(student.EmailAddress);
        passwordHasher.Received(1).VerifyHashedPassword(inputPassword, student.HashedPassword);
    }

    [Test]
    public async Task LoginAsStudent_ReturnsNotFound_WhenStudentDoesntExist()
    {
        // Arrange
        var repo = GetRepository();

        var email = Email.Create("student@test.com");
        repo.GetByEmail(email).Returns((Student?)null);

        var sut = GetSut();

        // Act
        var result = await sut.LoginAsStudent(email.Address, "pw");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task LoginAsStudent_ReturnsInvalidLogin_WhenPasswordIsWrong()
    {
        // Arrange
        var repo = GetRepository();
        var passwordHasher = GetPasswordHasher();

        var inputPassword = "incorrect";
        var student = Student.CreateTestStudent();

        repo.GetByEmail(student.EmailAddress).Returns(student);
        passwordHasher.VerifyHashedPassword(inputPassword, student.HashedPassword).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.LoginAsStudent(student.EmailAddress.Address, inputPassword);
        
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<InvalidLoginRequestException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).GetByEmail(Arg.Any<Email>());
    }

    [Test]
    public async Task CreateStudent_ReturnsStudent_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();
        var guidService = GetGuidGenerator();
        var passwordHasher = GetPasswordHasher();

        var password = "pw";
        var student = Student.CreateTestStudent();

        guidService.NewGuid().Returns(student.Id.Value);
        passwordHasher.HashPassword(password).Returns(student.HashedPassword);
        repo.Create(Arg.Any<Student>()).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.CreateStudent(
            student.StudentName,
            student.EmailAddress,
            password,
            student.PhoneNumber,
            student.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Id.Value, Is.EqualTo(student.Id.Value));
        Assert.That(result.Value.StudentName, Is.EqualTo(student.StudentName));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(student.EmailAddress));
        Assert.That(result.Value.HashedPassword, Is.EqualTo(student.HashedPassword));
        Assert.That(result.Value.PhoneNumber, Is.EqualTo(student.PhoneNumber));
        Assert.That(result.Value.SchoolId, Is.EqualTo(student.SchoolId));

        await repo.Received(1).Create(Arg.Any<Student>());
        await repo.Received(1).Save();
    }

    [Test]
    public async Task CreateStudent_ReturnsFailure_AndDoesNotSave_WhenCreationFails()
    {
        // Arrange
        var repo = GetRepository();
        var passwordHasher = GetPasswordHasher();

        var password = "pw";
        var student = Student.CreateTestStudent();

        passwordHasher.HashPassword(password).Returns(student.HashedPassword);
        repo.Create(Arg.Any<Student>()).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.CreateStudent(
            student.StudentName,
            student.EmailAddress,
            password,
            student.PhoneNumber,
            student.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<Exception>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Create(Arg.Any<Student>());
        await repo.DidNotReceive().Save();
    }

    [Test]
    public async Task GetStudentById_ReturnsStudent_WhenFound()
    {
        // Arrange
        var repo = GetRepository();

        var student = Student.CreateTestStudent();
        repo.Get(student.Id).Returns(student);

        var sut = GetSut();

        // Act
        var result = await sut.GetStudentById(student.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Id, Is.EqualTo(student.Id));
        Assert.That(result.Value.StudentName, Is.EqualTo(student.StudentName));
        Assert.That(result.Value.EmailAddress, Is.EqualTo(student.EmailAddress));

        await repo.Received(1).Get(Arg.Any<StudentKey>());
    }

    [Test]
    public async Task GetStudentById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var repo = GetRepository();

        var studentId = StudentKey.Create(Guid.NewGuid());

        var sut = GetSut();

        // Act
        var result = await sut.GetStudentById(studentId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(Arg.Any<StudentKey>());
    }

    [Test]
    public async Task GetAllStudents_ReturnsAllStudents_WhenPopulated()
    {
        // Arrange
        var repo = GetRepository();

        var student1 = Student.CreateTestStudent();
        var student2 = Student.CreateTestStudent();

        repo.GetAll().Returns([student1, student2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllStudents();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(2));
        Assert.That(result.Value, Does.Contain(student1));
        Assert.That(result.Value, Does.Contain(student2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllStudents_ReturnsEmpty_WhenEmpty()
    {
        // Arrange
        var repo = GetRepository();

        repo.GetAll().Returns(Array.Empty<Student>());

        var sut = GetSut();

        // Act
        var result = await sut.GetAllStudents();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllStudentsFromSchool_ReturnsStudents_WhenRepoHasData()
    {
        // Arrange
        var repo = GetRepository();

        var schoolGuid = Guid.NewGuid();
        var student1 = Student.CreateTestStudent(schoolGuid: schoolGuid);
        var student2 = Student.CreateTestStudent();

        repo.GetAll().Returns([student1, student2]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllStudentsFromSchool(student1.SchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count(), Is.EqualTo(1));
        Assert.That(result.Value, Does.Contain(student1));
        Assert.That(result.Value, Does.Not.Contain(student2));

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetAllStudentsFromSchool_ReturnsEmpty_WhenNoMatchingStudents()
    {
        // Arrange
        var repo = GetRepository();

        var student = Student.CreateTestStudent();
        var otherSchoolId = DrivingSchoolKey.Create(Guid.NewGuid());

        repo.GetAll().Returns([student]);

        var sut = GetSut();

        // Act
        var result = await sut.GetAllStudentsFromSchool(otherSchoolId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.Empty);

        await repo.Received(1).GetAll();
    }

    [Test]
    public async Task GetStudentDrivingSchoolId_ReturnsDrivingSchoolId_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();

        var student = Student.CreateTestStudent();
        repo.Get(student.Id).Returns(student);

        var sut = GetSut();

        // Act
        var result = await sut.GetStudentDrivingSchoolId(student.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(student.SchoolId));

        await repo.Received(1).Get(student.Id);
    }

    [Test]
    public async Task GetStudentDrivingSchoolId_ReturnsNotFound_WhenStudentNotExists()
    {
        // Arrange
        var repo = GetRepository();

        var studentId = StudentKey.Create(Guid.NewGuid());

        var sut = GetSut();

        // Act
        var result = await sut.GetStudentDrivingSchoolId(studentId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Get(studentId);
    }

    [Test]
    public async Task DeleteStudent_ReturnsSuccess_AndSaves_OnSuccess()
    {
        // Arrange
        var repo = GetRepository();
        
        var studentId = StudentKey.Create(Guid.NewGuid());
        repo.Delete(studentId).Returns(true);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteStudent(studentId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Error, Is.Null);

        await repo.Received(1).Delete(studentId);
        await repo.Received(1).Save();
    }

    [Test]
    public async Task DeleteStudent_ReturnsNotFound_AndDoesNotSave_WhenStudentNotFound()
    {
        // Arrange
        var repo = GetRepository();

        var studentId = StudentKey.Create(Guid.NewGuid());
        repo.Delete(studentId).Returns(false);

        var sut = GetSut();

        // Act
        var result = await sut.DeleteStudent(studentId);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.TypeOf<StudentNotFoundException>());
        Assert.That(result.Error!.Message, Is.Not.Null);

        await repo.Received(1).Delete(studentId);
        await repo.DidNotReceive().Save();
    }
}
