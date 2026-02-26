using System.Collections.Immutable;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class TheoryLesson : Entity
{
    public Guid SchoolId { get; }
    public Guid InstructorId { get; }
    public ImmutableArray<Student> Students { get; }
    public DateTime LessonDateTime { get; }
    public Money Price { get; }

    private TheoryLesson() : base(Guid.Empty) {} // EF
    
    public TheoryLesson(
        Guid id, 
        Guid schoolId, 
        DateTime lessonDateTime, 
        Money price, 
        Guid instructorId,
        IEnumerable<Student> students) : base(id)
    {
        // I don't know if it's right to do validation here
        var temp = students as Student[] ?? students.ToArray();
        if (temp.Distinct().Count() != temp.Length)
            throw new InvalidInputException("Cannot add duplicates of students to theory lesson");

        SchoolId = schoolId;
        LessonDateTime = lessonDateTime;
        Price = price;
        InstructorId = instructorId;
        Students = [..temp];
    }
}