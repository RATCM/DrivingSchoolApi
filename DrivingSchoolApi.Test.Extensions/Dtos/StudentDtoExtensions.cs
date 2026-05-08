using DrivingSchoolApi.DTOs.Student;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class StudentDtoExtensions
{
    extension(StudentRegistryDto registryDto)
    {
        public static StudentRegistryDto CreateTestStudent(Guid inviteId)
        {
            var name = new NameDto("Test", "Student");
            var email = "student@test.com";
            var phoneNumber = "11223344";
            var password = "1234";
            
            
            return new StudentRegistryDto(
                name,
                email, 
                phoneNumber,
                password,
                inviteId);
        }
    }
}