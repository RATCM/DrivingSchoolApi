using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.UnitTest.Entities;

public class DrivingLessonTests
{
    [Test]
    public void Create_Succeeds_WhenInputIsValid()
    {
        Assert.DoesNotThrow(() =>
            DrivingLesson.Create(
                DrivingLessonKey.Create(Guid.Empty),
                DrivingSchoolKey.Create(Guid.Empty),
                DrivingRoute.Create(DateTimeRange.Create(
                    new DateTime(2000, 1, 1),
                    new DateTime(2000, 1, 2)),
                    [CoordinatePoint.Create(1, 0, 0)]),
                Money.Create(10, "USD"),
                InstructorKey.Create(Guid.Empty),
                StudentKey.Create(Guid.Empty),
                Signature.Create([0]),
                Signature.Create([0])));
    }
}