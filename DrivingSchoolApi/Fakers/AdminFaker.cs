using Bogus;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class AdminFaker : Faker<Admin>
{
    private string? _password = null;
    
    private AdminFaker(int seed, IPasswordHasher<Admin> adminPasswordHasher)
    {
        UseSeed(seed)
            .CustomInstantiator(f => Admin.Create(
                AdminKey.Create(Guid.NewGuid()),
                Email.Create(f.Person.Email),
                adminPasswordHasher.HashPassword(_password ?? f.Random.AlphaNumeric(16))
            ));
    }

    public AdminFaker UsePassword(string? password)
    {
        _password = password;
        
        return this;
    }
    
    public static AdminFaker Create(int seed, IPasswordHasher<Admin> adminPasswordHasher)
    {
        return new AdminFaker(seed, adminPasswordHasher);
    }
}