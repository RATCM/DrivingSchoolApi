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
    
    [Test]
public async Task GetAllFromSchool_ReturnsOnlyStudentsFromRequestedSchool()
{
    // Arrange
    var dbContextOptions = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
        .UseInMemoryDatabase(databaseName: "DrivingSchoolDb_Test")
        .Options;

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

    await using (var context = new DrivingSchoolDbContext(dbContextOptions))
    {
        var studentRepository = new StudentRepository(context);
        await studentRepository.Create(student1);
        await studentRepository.Create(student2);
        await studentRepository.Create(student3);
        await studentRepository.Save();
    }

    // Act
    await using (var context = new DrivingSchoolDbContext(dbContextOptions))
    {
        var studentRepository = new StudentRepository(context);
        var recv = (await studentRepository.GetAllFromDrivingSchool(schoolA)).ToList();

        // Assert
        Assert.That(recv, Has.Count.EqualTo(2));
        Assert.That(recv.All(s => s.SchoolId.Equals(schoolA)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student1.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student2.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student3.Id)), Is.False);
    }
}

[Test]
public async Task GetAllFromSchool_ReturnsEmpty_When_NoStudentsForSchool()
{
    // Arrange
    var dbContextOptions = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
        .UseInMemoryDatabase(databaseName: $"DrivingSchoolDb_Test_{Guid.NewGuid()}")
        .Options;

    var existingSchool = DrivingSchoolKey.Create(Guid.NewGuid());
    var requestedSchool = DrivingSchoolKey.Create(Guid.NewGuid());

    var student = Student.Create(
        StudentKey.Create(Guid.NewGuid()),
        existingSchool,
        Name.Create("Only", "Student"),
        Email.Create("only@mail"),
        PasswordHash.Create("hash"),
        PhoneNumber.Create("44444444"));

    await using (var context = new DrivingSchoolDbContext(dbContextOptions))
    {
        var studentRepository = new StudentRepository(context);
        await studentRepository.Create(student);
        await studentRepository.Save();
    }

    // Act
    await using (var context = new DrivingSchoolDbContext(dbContextOptions))
    {
        var studentRepository = new StudentRepository(context);
        var recv = await studentRepository.GetAllFromDrivingSchool(requestedSchool);

        // Assert
        Assert.That(recv, Is.Empty);
    }
}

    
    
}