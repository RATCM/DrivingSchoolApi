using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingSchool : Entity
{
    private List<Student> _students = [];
    private List<Instructor> _instructors = [];
    
    public Address SchoolAddress { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public WebAddress WebAddress { get; private set; }
    public IReadOnlyList<Student> Students => _students.AsReadOnly();
    public IReadOnlyList<Instructor> Instructors => _instructors.AsReadOnly();
    
    private DrivingSchool() : base(Guid.Empty) {} // EF
    
    public DrivingSchool(Guid id, Address schoolAddress, PhoneNumber phoneNumber, WebAddress webAddress) : base(id)
    {
        SchoolAddress = schoolAddress;
        PhoneNumber = phoneNumber;
        WebAddress = webAddress;
    }

    public void AddStudent(Student student)
    {
        if(Students.Contains(student))
            throw new InvalidOperationException("Student already exists in this school");
        _students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        if(!_students.Remove(student))
            throw new InvalidOperationException("Student is not part of this school");
    }

    public void AddInstructor(Instructor instructor)
    {
        if(Instructors.Contains(instructor))
            throw new InvalidOperationException("Instructor already exists in this school");
        _instructors.Add(instructor);
    }
    
    public void RemoveInstructor(Instructor instructor)
    {
        if(!_instructors.Remove(instructor))
            throw new InvalidOperationException("Instructor is not part of this school");
    }
    
    
}