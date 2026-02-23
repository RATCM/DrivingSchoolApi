using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Student : Entity
{
    private List<TheoryLesson> _theoryLessons = [];
    private List<DrivingLesson> _drivingLessons = [];

    public Guid SchoolId { get; private set; }
    public Name StudentName { get; private set; }
    public Email EmailAddress { get; private set; }
    public PasswordHash HashedPassword { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<TheoryLesson> TheoryLessons => _theoryLessons.AsReadOnly();
    public IReadOnlyList<DrivingLesson> DrivingLessons => _drivingLessons.AsReadOnly();

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

    public void AddDrivingLesson(Guid id, DrivingRoute route, Money price, Guid instructorId)
    {
        var lesson = new DrivingLesson(id, this.SchoolId, route, price, instructorId, this.Id);
        
        _drivingLessons.Add(lesson);
    }
}