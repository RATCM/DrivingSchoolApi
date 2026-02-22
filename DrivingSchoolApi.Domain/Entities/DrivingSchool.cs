using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class DrivingSchool : Entity
{
    public Address SchoolAddress { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public WebAddress WebAddress { get; private set; }
    public List<Student> Students = [];
    public List<Instructor> Instructors = [];
    public List<DrivingLesson> DrivingLessons = [];
    public List<TheoryLesson> TheoryLessons = [];
    
    private DrivingSchool() : base(Guid.Empty) {} // EF
    
    public DrivingSchool(Guid id, Address schoolAddress, PhoneNumber phoneNumber, WebAddress webAddress) : base(id)
    {
        SchoolAddress = schoolAddress;
        PhoneNumber = phoneNumber;
        WebAddress = webAddress;
    }
}