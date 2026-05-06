using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class StudentRepositoryTests : TestClass
{
    [Test]
    public async Task CreateStudent_Fails_WhenNoDrivingSchoolExists()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("First", "Last"),
            Email.Create("test@mail"),
            PasswordHash.Create("someHash"),
            PhoneNumber.Create("12345678"));
        
        // Act
        var studentRepository = GetStudentRepository();
        var created = await studentRepository.Create(student);
        await studentRepository.Save();

        // Assert
        var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));

        Assert.That(created, Is.False);
        Assert.That(recv, Is.Null);
    }
    
    [Test]
    public async Task NullStudent_When_NotCreatedInDatabase()
    {
        // Act
        var studentRepository = GetStudentRepository();
        var recv = await studentRepository.Get(StudentKey.Create(Guid.Empty));
            
        // Assert
        Assert.That(recv, Is.Null);
    }
    
    
    [Test]
    public async Task RetrieveStudent_When_CreatedInDatabase()
    {
        // Arrange
        var school = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name"),
            StreetAddress.Create("1234", "test city", "test region", "test address"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool.com"),
            []);
        
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            school.Id,
            Name.Create("First", "Last"),
            Email.Create("test@mail"),
            PasswordHash.Create("someHash"),
            PhoneNumber.Create("12345678"));
        
        // Create driving school first
        var drivingSchoolRepository = GetDrivingSchoolRepository();
        await drivingSchoolRepository.Create(school);
        await drivingSchoolRepository.Save();
        
        // Act
        var studentRepository = GetStudentRepository();
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
        var schoolA = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("school A"),
            StreetAddress.Create("1234", "test city 1", "test region 1", "test address 1"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool1.com"),
            []);
        
        var schoolB = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("school B"),
            StreetAddress.Create("5678", "test city 2", "test region 2", "test address 2"),
            PhoneNumber.Create("22222222"),
            WebAddress.Create("testSchool2.com"),
            []);
        
        var student1 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
    
        var student2 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));
    
        var student3 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolB.Id,
            Name.Create("Charlie", "Clark"),
            Email.Create("charlie@mail"),
            PasswordHash.Create("hash3"),
            PhoneNumber.Create("33333333"));

        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();
        
        var studentRepository = GetStudentRepository();
        await studentRepository.Create(student1);
        await studentRepository.Create(student2);
        await studentRepository.Create(student3);
        await studentRepository.Save();
    
        // Act
        var recv = (await studentRepository.GetAllFromDrivingSchool(schoolA.Id)).ToList();

        // Assert
        Assert.That(recv, Has.Count.EqualTo(2));
        Assert.That(recv.All(s => s.SchoolId.Equals(schoolA.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student1.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student2.Id)), Is.True);
        Assert.That(recv.Any(s => s.Id.Equals(student3.Id)), Is.False);
    }
    
    [Test]
    public async Task GetAllFromSchool_ReturnsEmpty_When_NoStudentsForSchool()
    {
        // Arrange
        var existingSchool =  DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 1"),
            StreetAddress.Create("1234", "test city 1", "test region 1", "test address 1"),
            PhoneNumber.Create("11111111"),
            WebAddress.Create("testSchool1.com"),
            []);
        var requestedSchool = DrivingSchool.Create(
            DrivingSchoolKey.Create(Guid.NewGuid()),
            DrivingSchoolName.Create("test name 2"),
            StreetAddress.Create("5678", "test city 2", "test region 2", "test address 2"),
            PhoneNumber.Create("22222222"),
            WebAddress.Create("testSchool2.com"),
            []);
    
        var student = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            existingSchool.Id,
            Name.Create("Only", "Student"),
            Email.Create("only@mail"),
            PasswordHash.Create("hash"),
            PhoneNumber.Create("44444444"));

        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(existingSchool);
        await schoolRepository.Save();
        
        var studentRepository = GetStudentRepository();
        await studentRepository.Create(student);
        await studentRepository.Save();
    
        // Act
        var recv = await studentRepository.GetAllFromDrivingSchool(requestedSchool.Id);

        // Assert
        Assert.That(recv, Is.Empty);
    }

    [Test]
    public async Task Update_ReturnsTrue_WhenStudentExists()
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

        var existingStudent = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
        
        var newStudent = Student.Create(
            existingStudent.Id,
            schoolB.Id,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("11111111"));

        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();
        
        var studentRepository = GetStudentRepository();
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
        
        var studentRepository = GetStudentRepository();
        
        // Act
        var recv = await studentRepository.Update(newStudent);
        await studentRepository.Save();
        
        // Assert
        Assert.That(recv, Is.False);
    }
    
    
    [Test]
    public async Task DeleteDrivingSchool_AlsoDeletes_Students()
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
        
        var student1 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
    
        var student2 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));
    
        var student3 = Student.Create(
            StudentKey.Create(Guid.NewGuid()),
            schoolB.Id,
            Name.Create("Charlie", "Clark"),
            Email.Create("charlie@mail"),
            PasswordHash.Create("hash3"),
            PhoneNumber.Create("33333333"));
        
        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();

        var studentRepository = GetStudentRepository();
        await studentRepository.Create(student1);
        await studentRepository.Create(student2);
        await studentRepository.Create(student3);
        await studentRepository.Save();
        
        // Act
        var deleted = await schoolRepository.Delete(schoolA.Id);
        await schoolRepository.Save();

        var schoolAStudents = await studentRepository.GetAllFromDrivingSchool(schoolA.Id);
        var schoolBStudents = await studentRepository.GetAllFromDrivingSchool(schoolB.Id);

        // Assert
        Assert.That(deleted, Is.True);
        Assert.That(schoolAStudents, Is.Empty);
        Assert.That(schoolBStudents, Is.Not.Empty);
    }
}
