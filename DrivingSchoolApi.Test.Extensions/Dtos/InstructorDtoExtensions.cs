using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class InstructorDtoExtensions
{
    extension(InstructorRegistryDto registryDto)
    {
        public static InstructorRegistryDto CreateTestInstructor(Guid schoolId)
        {
            var name = new NameDto("Test", "Instructor");
            var email = "instructor@test.com";
            var phoneNumber = "11223344";
            var password = "1234";

            return new InstructorRegistryDto(
                schoolId,
                name,
                email,
                phoneNumber,
                password);
        }
    }
}