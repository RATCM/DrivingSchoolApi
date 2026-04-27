using System.Collections.Immutable;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class TheoryLesson : Entity<TheoryLessonKey>
{
    public required DrivingSchoolKey SchoolId { get; init; }
    public required InstructorKey? InstructorId { get; init; }
    public required StudentKey? StudentId { get; init; }
    public required DateTime LessonDateTime { get; init; }
    public required Money Price { get; init; }
    public required Signature InstructorSignature { get; init; }
    public required Signature StudentSignature { get; init; }

    
    //TODO Instructor signature

    private TheoryLesson() {} // EF
    
    public static TheoryLesson Create(
        TheoryLessonKey id,
        DrivingSchoolKey schoolId,
        DateTime lessonDateTime,
        Money price,
        InstructorKey instructorId,
        StudentKey studentId,
        Signature instructorSignature,
        Signature studentSignature)
    {
        return new TheoryLesson
        {
            Id = id,
            SchoolId = schoolId,
            LessonDateTime = lessonDateTime,
            Price = price,
            InstructorId = instructorId,
            StudentId = studentId,
            InstructorSignature = instructorSignature,
            StudentSignature = studentSignature
        };
    }
}