using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class InstructorRepositoryTests
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
    public async Task Update_ReturnsTrue_WhenInstructorExists()
    {
        // Arrange
        var existingInstructor = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
        
        var newInstructor = Instructor.Create(
            existingInstructor.Id,
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));

        
        var instructorRepository = new InstructorRepository(_dbContext);

        await instructorRepository.Create(existingInstructor);
        await instructorRepository.Save();
        
        // Act
        var recv = await instructorRepository.Update(newInstructor);
        await instructorRepository.Save();
        
        var recvInstructor = await instructorRepository.Get(newInstructor.Id);
        
        // Assert
        Assert.That(recv, Is.True);
        Assert.That(recvInstructor, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(recvInstructor.Id, Is.EqualTo(newInstructor.Id));
            Assert.That(recvInstructor.SchoolId, Is.EqualTo(newInstructor.SchoolId));
            Assert.That(recvInstructor.PhoneNumber, Is.EqualTo(newInstructor.PhoneNumber));
            Assert.That(recvInstructor.EmailAddress, Is.EqualTo(newInstructor.EmailAddress));
            Assert.That(recvInstructor.InstructorName, Is.EqualTo(newInstructor.InstructorName));
            Assert.That(recvInstructor.Calender, Is.EqualTo(newInstructor.Calender));
            Assert.That(recvInstructor.HashedPassword, Is.EqualTo(newInstructor.HashedPassword));
        }
        );
    }
    
    [Test]
    public async Task Update_ReturnsFalse_WhenInstructorDoesntExist()
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