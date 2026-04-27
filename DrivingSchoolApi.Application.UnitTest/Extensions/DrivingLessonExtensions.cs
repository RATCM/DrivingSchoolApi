using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class DrivingLessonExtensions
{
    extension(DrivingLesson drivingLesson)
    {
        public static DrivingLesson CreateTestLesson(
            Guid? lessonGuid = null, 
            Guid? schoolGuid = null,
            Guid? studentGuid = null,
            Guid? instructorGuid = null)
        {
            lessonGuid ??= Guid.NewGuid();
            schoolGuid ??= Guid.NewGuid();
            studentGuid ??= Guid.NewGuid();
            instructorGuid ??= Guid.NewGuid();
        
            var dateTimeProvider = Substitute.For<IDateTimeProviderService>();
        
            var lessonId = DrivingLessonKey.Create(lessonGuid.Value);
            var schoolId = DrivingSchoolKey.Create(schoolGuid.Value);
            var instructorId = InstructorKey.Create(instructorGuid.Value);
            var studentId = StudentKey.Create(studentGuid.Value);
        
            var dateTimeRange = DateTimeRange.Create(dateTimeProvider.Now(), dateTimeProvider.Now().AddHours(1));
            var point1 = CoordinatePoint.Create(1, 0f, 0f);
            var point2 = CoordinatePoint.Create(2, 1f, 1f);
            var point3 = CoordinatePoint.Create(3, 2f, 2f);
            var coordinatePoints = new[] { point1, point2, point3 };
            var route = DrivingRoute.Create(dateTimeRange, coordinatePoints);
        
            var instructorSig = Signature.Create([1, 2, 3]);
            var studentSig = Signature.Create([4, 5, 6]);

            return DrivingLesson.Create(
                lessonId,
                schoolId,
                route,
                Money.Create(100, "DKK"),
                instructorId,
                studentId,
                instructorSig,
                studentSig,
                0);
        }
    }
}
