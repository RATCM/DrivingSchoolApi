using System.Collections.Immutable;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class TheoryLesson : Entity<TheoryLessonKey>
{
    public required DrivingSchoolKey SchoolId { get; init; }
    public required InstructorKey InstructorId { get; init; }
    public required ImmutableArray<StudentKey> StudentIds { get; init; }
    public required DateTime LessonDateTime { get; init; }
    public required Money Price { get; init; }
    public required Signature InstructorSignature { get; init; }

    private TheoryLesson() {} // EF
    
    public static TheoryLesson Create(
        TheoryLessonKey id,
        DrivingSchoolKey schoolId,
        DateTime lessonDateTime,
        Money price,
        InstructorKey instructorId,
        IEnumerable<StudentKey> studentIds,
        Signature instructorSignature)
    {
        // I don't know if it's right to do validation here
        var temp = studentIds as StudentKey[] ?? studentIds.ToArray();
        if (temp.Distinct().Count() != temp.Length)
            throw new InvalidInputException("Cannot add duplicates of students to theory lesson");

        return new TheoryLesson
        {
            Id = id,
            SchoolId = schoolId,
            LessonDateTime = lessonDateTime,
            Price = price,
            InstructorId = instructorId,
            StudentIds = [..temp],
            InstructorSignature = instructorSignature
        };
    }
}