using DrivingSchoolApi.DTOs.Admin;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class AdminDtoExtensions
{
    extension(AdminRegistryDto adminDto)
    {
        public static AdminRegistryDto CreateTestAdmin(string email = "admin@test.com", string password = "password")
        {
            return new AdminRegistryDto(email, password);
        }

    }
}