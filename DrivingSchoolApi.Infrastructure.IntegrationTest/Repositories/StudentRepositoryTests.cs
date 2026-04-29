using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class StudentRepositoryTests
{
    private DrivingSchoolDbContext _dbContext;
    
    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
            .UseInMemoryDatabase(databaseName: "DrivingSchoolDb_Test")
            .Options;
        
        _dbContext = new DrivingSchoolDbContext(dbContextOptions);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task NullStudent_When_NotCreatedInDatabase()
    {
        // Act
        var studentRepository = new StudentRepository(_dbContext);
        var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));
            
        // Assert
        Assert.That(recv, Is.Null);
    }
    
    
    [Test]
    public async Task RetrieveStudent_When_CreatedInDatabase()
    {
        // Arrange
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("First", "Last"),
            Email.Create("test@mail"),
            PasswordHash.Create("someHash"),
            PhoneNumber.Create("12345678"));

        // Act
        var studentRepository = new StudentRepository(_dbContext);
        await studentRepository.Create(student);
        await studentRepository.Save();

        // Assert
        var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));
        
        Assert.That(recv, Is.Not.Null);
        Assert.That(recv, Is.EqualTo(student));
    }
    
    [Test]
    public async Task GetAllFromSchool_ReturnsOnlyStudentsFromRequestedSchool()
    {
        // Arrange
        var schoolA = DrivingSchoolKey.Create(Guid.NewGuid());
        var schoolB = DrivingSchoolKey.Create(Guid.NewGuid());
    
        var student1 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
    
        var student2 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));
    
        var student3 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolB,
            Name.Create("Charlie", "Clark"),
            Email.Create("charlie@mail"),
            PasswordHash.Create("hash3"),
            PhoneNumber.Create("33333333"));
    
        var studentRepository = new StudentRepository(_dbContext);
        await studentRepository.Create(student1);
        await studentRepository.Create(student2);
        await studentRepository.Create(student3);
        await studentRepository.Save();
    
        // Act
        var recv = (await studentRepository.GetAllFromDrivingSchool(schoolA)).ToList();

        // Assert
        Assert.That(recv, Has.Count.EqualTo(2));
        Assert.That(recv.All(s => s.SchoolId.Equals(schoolA)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student1.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student2.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student3.Id)), Is.False);
    }
    
    [Test]
    public async Task GetAllFromSchool_ReturnsEmpty_When_NoStudentsForSchool()
    {
        // Arrange
        var existingSchool = DrivingSchoolKey.Create(Guid.NewGuid());
        var requestedSchool = DrivingSchoolKey.Create(Guid.NewGuid());
    
        var student = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            existingSchool,
            Name.Create("Only", "Student"),
            Email.Create("only@mail"),
            PasswordHash.Create("hash"),
            PhoneNumber.Create("44444444"));
    
        var studentRepository = new StudentRepository(_dbContext);
        await studentRepository.Create(student);
        await studentRepository.Save();
    
        // Act
        var recv = await studentRepository.GetAllFromDrivingSchool(requestedSchool);

        // Assert
        Assert.That(recv, Is.Empty);
    }

    [Test]
    public async Task Update_ReturnsTrue_WhenStudentExists()
    {
        // Arrange
        var existingStudent = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
        
        var newStudent = Student.Create(
            existingStudent.Id,
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));

        
        var studentRepository = new StudentRepository(_dbContext);

        await studentRepository.Create(existingStudent);
        await studentRepository.Save();
        
        // Act
        var recv = await studentRepository.Update(newStudent);
        await studentRepository.Save();
        
        var recvStudent = await studentRepository.Get(newStudent.Id);
        
        // Assert
        Assert.That(recv, Is.True);
        Assert.That(recvStudent, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(recvStudent.Id, Is.EqualTo(newStudent.Id));
            Assert.That(recvStudent.SchoolId, Is.EqualTo(newStudent.SchoolId));
            Assert.That(recvStudent.PhoneNumber, Is.EqualTo(newStudent.PhoneNumber));
            Assert.That(recvStudent.EmailAddress, Is.EqualTo(newStudent.EmailAddress));
            Assert.That(recvStudent.StudentName, Is.EqualTo(newStudent.StudentName));
            Assert.That(recvStudent.Calender, Is.EqualTo(newStudent.Calender));
            Assert.That(recvStudent.HashedPassword, Is.EqualTo(newStudent.HashedPassword));
        }
        );
    }
    
    [Test]
    public async Task Update_ReturnsFalse_WhenStudentDoesntExist()
    {
        // Arrange
        var newStudent = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));
        
        var studentRepository = new StudentRepository(_dbContext);
        
        // Act
        var recv = await studentRepository.Update(newStudent);
        await studentRepository.Save();
        
        // Assert
        Assert.That(recv, Is.False);
    }
}
