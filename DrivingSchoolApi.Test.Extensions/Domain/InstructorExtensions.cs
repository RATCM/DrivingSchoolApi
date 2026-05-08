using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Test.Extensions.Domain;

public static class InstructorExtensions
{
    extension(Instructor instructor)
    {
        public static Instructor CreateTestInstructor(
            Guid? instructorGuid = null, 
            Guid? schoolGuid = null)
        {
            instructorGuid ??= Guid.NewGuid();
            schoolGuid ??= Guid.NewGuid();
        
            var instructorId = InstructorKey.Create(instructorGuid.Value);
            var schoolId = DrivingSchoolKey.Create(schoolGuid.Value);
            var name = Name.Create("John", "Doe");
            var email = Email.Create("test@email.dk");
            var passwordHash = PasswordHash.Create("password");
            var phoneNumber = PhoneNumber.Create("0123456789");
        
            return Instructor.Create(
                instructorId, 
                schoolId, 
                name,
                email,
                passwordHash,
                phoneNumber);
        }
    }
}
