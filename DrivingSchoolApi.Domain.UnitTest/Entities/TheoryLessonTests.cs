using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Exceptions;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.Entities;

public class TheoryLessonTests
{
    [Test]
    public void TheoryLesson_Fails_WhenStudentIdsContainsDuplicates()
    {
        Assert.Throws<InvalidInputException>(
            () => TheoryLesson.Create(
                TheoryLessonKey.Create(Guid.Empty),
                DrivingSchoolKey.Create(Guid.Empty),
                DateTime.UnixEpoch,
                Money.Create(10, "USD"),
                InstructorKey.Create(Guid.Empty),
                [
                    StudentKey.Create(Guid.Empty), 
                    StudentKey.Create(Guid.Empty)
                ],
                Signature.Create([0])));
    }
    
    [Test]
    public void TheoryLesson_Succeeds_WhenStudentIdsAreDistinct()
    {
        Assert.DoesNotThrow(
            () => TheoryLesson.Create(
                TheoryLessonKey.Create(Guid.Empty),
                DrivingSchoolKey.Create(Guid.Empty),
                DateTime.UnixEpoch,
                Money.Create(10, "USD"),
                InstructorKey.Create(Guid.Empty),
                [
                    StudentKey.Create(Guid.Empty), 
                    StudentKey.Create(Guid.AllBitsSet)
                ],
                Signature.Create([0])));
    }
}
