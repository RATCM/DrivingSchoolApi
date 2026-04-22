using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class CompletedCourseExtensions
{
    extension(CompletedCourse completedCourse)
    {
        public static CompletedCourse CreateTestCompletedCourse(
            Guid? id = null,
            Guid? schoolId = null,
            Guid? studentId = null,
            DateTime? completionDate = null,
            CourseCompletionReason? reason = null)
        {
            id ??= Guid.NewGuid();
            schoolId ??= Guid.NewGuid();
            studentId ??= Guid.NewGuid();
            var price = Money.Create(100, "DKK");
            completionDate ??= new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            reason ??= CourseCompletionReason.Finished;
            
            return CompletedCourse.Create(
                CompletedCourseKey.Create(id.Value),
                DrivingSchoolKey.Create(schoolId.Value),
                StudentKey.Create(studentId.Value),
                price,
                completionDate.Value,
                reason.Value);
        }
    }
}