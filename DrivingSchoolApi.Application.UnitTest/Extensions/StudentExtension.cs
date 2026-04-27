using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class StudentExtension
{
    extension(Student student)
    {
        public static Student CreateTestStudent(Guid? studentGuid = null, Guid? schoolGuid = null)
        {
            studentGuid ??= Guid.NewGuid();
            schoolGuid ??= Guid.NewGuid();

            var studentId = StudentKey.Create(studentGuid.Value);
            var drivingSchoolId = DrivingSchoolKey.Create(schoolGuid.Value);
            var name = Name.Create("Test", "Student");
            var email = Email.Create("test@student.dk");
            var passwordHash = PasswordHash.Create("password");
            var phoneNumber = PhoneNumber.Create("0123456789");
            
            return Student.Create(
                studentId,
                drivingSchoolId,
                name,
                email,
                passwordHash,
                phoneNumber);
        }
    }
}
