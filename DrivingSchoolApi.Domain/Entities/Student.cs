using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Student : Entity<StudentKey>
{
    public DrivingSchoolKey SchoolId { get; private set; } = null!;
    public Name StudentName { get; private set; } = null!;
    public Email EmailAddress { get; private set; } = null!;
    public PasswordHash HashedPassword { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;

    private Student() {} // EF

    public static Student Create(
        StudentKey id, 
        DrivingSchoolKey schoolId, 
        Name name, 
        Email email, 
        PasswordHash hashedPassword, 
        PhoneNumber phoneNumber)
    {
        return new Student
        {
            Id = id,
            SchoolId = schoolId,
            StudentName = name,
            EmailAddress = email,
            HashedPassword = hashedPassword,
            PhoneNumber = phoneNumber
        };
    }

    public void ChangeSchool(DrivingSchoolKey schoolId)
    {
        if (SchoolId.Equals(schoolId))
            throw new InvalidOperationException("Can't change to the same school to the same school");
        SchoolId = schoolId;
    }

    public void ChangeName(Name newName)
    {
        if (StudentName.Equals(newName))
            throw new InvalidOperationException("Can't change to the same name to the same name");
        StudentName = newName;
    }

    public void ChangeEmail(Email newEmail)
    {
        if (EmailAddress.Equals(newEmail))
            throw new InvalidOperationException("Can't change to the same email to the same email");
        EmailAddress = newEmail;
    }

    public void ChangePasswordHash(PasswordHash newPasswordHash)
    {
        // This will probably never run because the passwords are also salted
        // so even if the same password is provided, it will probably
        // result in a different hash
        if (HashedPassword.Equals(newPasswordHash))
            throw new InvalidOperationException("Can't change the same password to the same password");
        HashedPassword = newPasswordHash;
    }

    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (PhoneNumber.Equals(newPhoneNumber))
            throw new InvalidOperationException("Can't change the same phone number to the same phone number");
        PhoneNumber = newPhoneNumber;
    }
}