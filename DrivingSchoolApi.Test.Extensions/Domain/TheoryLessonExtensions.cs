using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Test.Extensions.Domain;

public static class TheoryLessonExtensions
{
    extension(TheoryLesson theoryLesson)
    {
        public static TheoryLesson CreateTestTheoryLesson(
            DateTime? dateTime = null,
            Guid? id = null, 
            Guid? schoolId = null,
            Guid? instructorId = null,
            Guid? studentId = null
            )
        {
            dateTime ??=  new DateTime(2026, 01, 01, 10, 00, 00);;
            id ??= Guid.NewGuid();
            schoolId ??= Guid.NewGuid();
            instructorId ??= Guid.NewGuid();
            studentId ??= Guid.NewGuid();
            var price = Money.Create(100, "DKK");
            var instructorSignature = Signature.Create([0]);
            var studentSignature = Signature.Create([0]);
            
            return TheoryLesson.Create(
                TheoryLessonKey.Create(id.Value),
                DrivingSchoolKey.Create(schoolId.Value),
                dateTime.Value,
                price,
                InstructorKey.Create(instructorId.Value),
                StudentKey.Create(studentId.Value),
                instructorSignature,
                studentSignature);
        }
    }
}
