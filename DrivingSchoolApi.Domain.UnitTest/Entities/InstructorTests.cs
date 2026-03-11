using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.Entities;

public class InstructorTests
{
    [Test]
    public void ChangeSchool_Fails_WhenSchoolIsIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => instructor.ChangeSchool(DrivingSchoolKey.Create(Guid.Empty)));
    }
    
    [Test]
    public void ChangeSchool_Succeeds_WhenSchoolIsNotIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => instructor.ChangeSchool(DrivingSchoolKey.Create(Guid.AllBitsSet)));
    }
    
    [Test]
    public void ChangeName_Fails_WhenNameIsIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => instructor.ChangeName(Name.Create("Test", "Instructor")));
    }
    
    [Test]
    public void ChangeName_Succeeds_WhenNameIsNotIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test1", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => instructor.ChangeName(Name.Create("Test2", "Instructor")));
    }
    
    [Test]
    public void ChangeEmail_Fails_WhenEmailIsIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => instructor.ChangeEmail(Email.Create("test@mail")));
    }
    
    [Test]
    public void ChangeEmail_Succeeds_WhenEmailIsNotIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test1@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => instructor.ChangeEmail(Email.Create("test2@mail")));
    }

    [Test]
    public void ChangePasswordHash_Fails_WhenPasswordHashIsIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => instructor.ChangePasswordHash(PasswordHash.Create("SomeHash")));
    }
    
    [Test]
    public void ChangePasswordHash_Succeeds_WhenPasswordHashIsNotIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash1"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => instructor.ChangePasswordHash(PasswordHash.Create("SomeHash2")));
    }
    
    [Test]
    public void ChangePhoneNumber_Fails_WhenPhoneNumberIsIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => instructor.ChangePhoneNumber(PhoneNumber.Create("12345678")));
    }
    
    [Test]
    public void ChangePhoneNumber_Succeeds_WhenPhoneNumberIsNotIdentical()
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Instructor"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => instructor.ChangePhoneNumber(PhoneNumber.Create("12345679")));
    }

}