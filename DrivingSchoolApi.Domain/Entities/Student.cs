using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Student : Entity
{
    public Guid SchoolId { get; private set; }
    public Name StudentName { get; private set; }
    public Email EmailAddress { get; private set; }
    public PasswordHash HashedPassword { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public List<TheoryLesson> TheoryLessons = [];
    public List<DrivingLesson> DrivingLessons = [];

    private Student() : base(Guid.Empty) {} // EF
    
    public Student(
        Guid id, 
        Guid schoolId, 
        Name name, 
        Email email, 
        PasswordHash hashedPassword, 
        PhoneNumber phoneNumber) : base(id)
    {
        SchoolId = schoolId;
        StudentName = name;
        EmailAddress = email;
        HashedPassword = hashedPassword;
        PhoneNumber = phoneNumber;
    }

}