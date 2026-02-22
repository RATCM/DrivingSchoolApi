using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Instructor : Entity
{
    public Guid SchoolId { get; private set; }
    public Name InstructorName { get; private set; }
    public Email EmailAddress { get; private set; }
    public PasswordHash HashedPassword { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public List<TheoryLesson> TheoryLessons = [];
    public List<DrivingLesson> DrivingLessons = [];

    private Instructor() : base(Guid.Empty) {} // EF
    
    public Instructor(
        Guid id, 
        Guid schoolId, 
        Name name, 
        Email email, 
        PasswordHash hashedPassword, 
        PhoneNumber phoneNumber) : base(id)
    {
        SchoolId = schoolId;
        InstructorName = name;
        EmailAddress = email;
        HashedPassword = hashedPassword;
        PhoneNumber = phoneNumber;
    }
    
}