using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Test.Extensions.Domain;

public static class AdminExtensions
{
    extension(Admin admin)
    {
        public static Admin CreateTestAdmin(Guid? id = null, string email = "admin@test.com", string password = "password")
        {
            id ??= Guid.NewGuid();
            
            var adminKey = AdminKey.Create(id.Value);
            var adminEmail = Email.Create(email);
            var hashedPassword = PasswordHash.Create(password);

            return Admin.Create(
                adminKey,
                adminEmail,
                hashedPassword);
        }
    }
}
