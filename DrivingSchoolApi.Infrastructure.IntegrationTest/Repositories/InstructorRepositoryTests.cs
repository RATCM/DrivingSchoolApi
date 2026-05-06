using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest.Repositories;

public class InstructorRepositoryTests : TestClass
{
    [Test]
    public async Task Update_ReturnsTrue_WhenInstructorExists()
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

        var existingInstructor = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
        
        var newInstructor = Instructor.Create(
            existingInstructor.Id,
            schoolB.Id,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("11111111"));


        var instructorRepository = GetInstructorRepository();

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
        var newInstructor = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            DrivingSchoolKey.Create(Guid.NewGuid()),
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));

        var instructorRepository = GetInstructorRepository();
        
        // Act
        var recv = await instructorRepository.Update(newInstructor);
        await instructorRepository.Save();
        
        // Assert
        Assert.That(recv, Is.False);
    }
    
    [Test]
    public async Task DeleteDrivingSchool_AlsoDeletes_Instructors()
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
        
        var instructor1 = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Alice", "Anderson"),
            Email.Create("alice@mail"),
            PasswordHash.Create("hash1"),
            PhoneNumber.Create("11111111"));
    
        var instructor2 = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            schoolA.Id,
            Name.Create("Bob", "Brown"),
            Email.Create("bob@mail"),
            PasswordHash.Create("hash2"),
            PhoneNumber.Create("22222222"));
    
        var instructor3 = Instructor.Create(
            InstructorKey.Create(Guid.NewGuid()),
            schoolB.Id,
            Name.Create("Charlie", "Clark"),
            Email.Create("charlie@mail"),
            PasswordHash.Create("hash3"),
            PhoneNumber.Create("33333333"));
        
        var schoolRepository = GetDrivingSchoolRepository();
        await schoolRepository.Create(schoolA);
        await schoolRepository.Create(schoolB);
        await schoolRepository.Save();

        var instructorRepository = GetInstructorRepository();
        await instructorRepository.Create(instructor1);
        await instructorRepository.Create(instructor2);
        await instructorRepository.Create(instructor3);
        await instructorRepository.Save();
        
        // Act
        var deleted = await schoolRepository.Delete(schoolA.Id);
        await schoolRepository.Save();

        var allInstructors = (await instructorRepository.GetAll()).ToList();
        
        var schoolAInstructors = allInstructors.Where(x => x.SchoolId.Equals(schoolA.Id));
        var schoolBInstructors = allInstructors.Where(x => x.SchoolId.Equals(schoolB.Id));

        // Assert
        Assert.That(deleted, Is.True);
        Assert.That(schoolAInstructors, Is.Empty);
        Assert.That(schoolBInstructors, Is.Not.Empty);
    }

}