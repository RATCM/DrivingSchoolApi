using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class StudentRepositoryTests
{
    
    [Test]
    public async Task NullStudent_When_NotCreatedInDatabase()
    {
        // Arrange
        var dbContextOptions = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
            .UseInMemoryDatabase(databaseName: "DrivingSchoolDb_Test")
            .Options;

        await using var context = new DrivingSchoolDbContext(dbContextOptions);
        
        // Act
        var studentRepository = new StudentRepository(context);
        var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));
            
        // Assert
        Assert.That(recv, Is.Null);
    }
    
    
    [Test]
    public async Task RetrieveStudent_When_CreatedInDatabase()
    {
        // Arrange
        var dbContextOptions = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
            .UseInMemoryDatabase(databaseName: "DrivingSchoolDb_Test")
            .Options;

        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("First", "Last"),
            Email.Create("test@mail"),
            PasswordHash.Create("someHash"),
            PhoneNumber.Create("12345678"));

        // Act
        await using (var context = new DrivingSchoolDbContext(dbContextOptions))
        {
            var studentRepository = new StudentRepository(context);
            await studentRepository.Create(student);
            await studentRepository.Save();
        }

        // Assert
        await using (var context = new DrivingSchoolDbContext(dbContextOptions))
        {
            var studentRepository = new StudentRepository(context);
            var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));
            
            Assert.That(recv, Is.Not.Null);
            Assert.That(recv, Is.EqualTo(student));
        }
    }
    
    
}