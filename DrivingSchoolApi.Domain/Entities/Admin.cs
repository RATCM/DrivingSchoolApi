using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Domain.Entities;

public sealed class Admin : Entity<AdminKey>
{
    public Email EmailAddress { get; private set; } = null!;
    public PasswordHash HashedPassword { get; private set; } = null!;
    
    private Admin() {}

    public static Admin Create(
        AdminKey adminId, 
        Email email,
        PasswordHash hashedPassword)
    {
        return new Admin
        {
            Id = adminId,
            EmailAddress = email,
            HashedPassword = hashedPassword
        };
    }
}