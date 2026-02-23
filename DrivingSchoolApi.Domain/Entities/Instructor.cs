using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Instructor : Entity
{
    private List<TheoryLesson> _theoryLessons = [];
    private List<DrivingLesson> _drivingLessons = [];

    public Guid SchoolId { get; private set; }
    public Name InstructorName { get; private set; }
    public Email EmailAddress { get; private set; }
    public PasswordHash HashedPassword { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<TheoryLesson> TheoryLessons => _theoryLessons.AsReadOnly();
    public IReadOnlyList<DrivingLesson> DrivingLessons => _drivingLessons.AsReadOnly();

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
    
    public void AddDrivingLesson(Guid id, DrivingRoute route, Money price, Guid studentId)
    {
        var lesson = new DrivingLesson(id, SchoolId, route, price, this.Id, studentId);
        
        _drivingLessons.Add(lesson);
    }

    public void AddTheoryLesson(Guid id, DateTime lessonDateTime, Money price, IEnumerable<Student> students)
    {
        var lesson = new TheoryLesson(id, this.SchoolId, lessonDateTime, price, this.Id, students);
        
        _theoryLessons.Add(lesson);
    }
}