using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using NUnit.Framework.Constraints;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

public static class TheoryLessonExtensions
{
    extension(TheoryLesson theoryLesson)
    {
        public static TheoryLesson CreateTestTheoryLesson(
            DateTime dateTime,
            Guid? id = null, 
            Guid? schoolId = null,
            Guid? instructorId = null
            )
        {
            id ??= Guid.NewGuid();
            schoolId ??= Guid.NewGuid();
            instructorId ??= Guid.NewGuid();
            var price = Money.Create(100, "DKK");
            var studentId = StudentKey.Create(Guid.NewGuid());
            var instructorSignature = Signature.Create([0]);
            var studentSignature = Signature.Create([0]);
            
            return TheoryLesson.Create(
                TheoryLessonKey.Create(id.Value),
                DrivingSchoolKey.Create(schoolId.Value),
                dateTime,
                price,
                InstructorKey.Create(instructorId.Value),
                studentId,
                instructorSignature,
                studentSignature);
        }
    }
}
