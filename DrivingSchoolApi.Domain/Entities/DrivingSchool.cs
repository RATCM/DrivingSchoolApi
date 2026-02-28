using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingSchool : Entity
{
    private List<Student> _students = [];
    private List<Instructor> _instructors = [];
    
    public DrivingSchoolName DrivingSchoolName { get; private set; }
    public StreetAddress SchoolStreetAddress { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public WebAddress WebAddress { get; private set; }
    public Money PackagePrice { get; private set; }
    public IReadOnlyList<Student> Students => _students.AsReadOnly();
    public IReadOnlyList<Instructor> Instructors => _instructors.AsReadOnly();
    private DrivingSchool() : base(Guid.Empty) {} // EF
    
    public DrivingSchool(Guid id, DrivingSchoolName drivingSchoolName, StreetAddress schoolStreetAddress, PhoneNumber phoneNumber, WebAddress webAddress, Money packagePrice) : base(id)
    {
        DrivingSchoolName = drivingSchoolName;
        SchoolStreetAddress = schoolStreetAddress;
        PhoneNumber = phoneNumber;
        WebAddress = webAddress;
        PackagePrice = packagePrice;
    }

    public void ChangeName(DrivingSchoolName newName)
    {
        if (newName == this.DrivingSchoolName)
        {
            throw new InvalidOperationException("Can't change name to the same name");
        }
        this.DrivingSchoolName = newName;
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