using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.Entities;

public class StudentTests
{
    [Test]
    public void ChangeSchool_Fails_WhenSchoolIsIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => student.ChangeSchool(DrivingSchoolKey.Create(Guid.Empty)));
    }
    
    [Test]
    public void ChangeSchool_Succeeds_WhenSchoolIsNotIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => student.ChangeSchool(DrivingSchoolKey.Create(Guid.AllBitsSet)));
    }
    
    [Test]
    public void ChangeName_Fails_WhenNameIsIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => student.ChangeName(Name.Create("Test", "Student")));
    }
    
    [Test]
    public void ChangeName_Succeeds_WhenNameIsNotIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test1", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => student.ChangeName(Name.Create("Test2", "Student")));
    }
    
    [Test]
    public void ChangeEmail_Fails_WhenEmailIsIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => student.ChangeEmail(Email.Create("test@mail")));
    }
    
    [Test]
    public void ChangeEmail_Succeeds_WhenEmailIsNotIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test1@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => student.ChangeEmail(Email.Create("test2@mail")));
    }

    [Test]
    public void ChangePasswordHash_Fails_WhenPasswordHashIsIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => student.ChangePasswordHash(PasswordHash.Create("SomeHash")));
    }
    
    [Test]
    public void ChangePasswordHash_Succeeds_WhenPasswordHashIsNotIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash1"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => student.ChangePasswordHash(PasswordHash.Create("SomeHash2")));
    }
    
    [Test]
    public void ChangePhoneNumber_Fails_WhenPhoneNumberIsIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.Throws<InvalidOperationException>(
            () => student.ChangePhoneNumber(PhoneNumber.Create("12345678")));
    }
    
    [Test]
    public void ChangePhoneNumber_Succeeds_WhenPhoneNumberIsNotIdentical()
    {
        var student = Student.Create(
            StudentKey.Create(Guid.Empty),
            DrivingSchoolKey.Create(Guid.Empty),
            Name.Create("Test", "Student"),
            Email.Create("test@mail"),
            PasswordHash.Create("SomeHash"),
            PhoneNumber.Create("12345678"));

        Assert.DoesNotThrow(
            () => student.ChangePhoneNumber(PhoneNumber.Create("12345679")));
    }
}