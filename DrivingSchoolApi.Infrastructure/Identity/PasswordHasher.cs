using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace DrivingSchoolApi.Infrastructure.Identity;

// Not really any need for this to be generic honestly
internal class PasswordHasher<T> : Application.Services.IPasswordHasher<T> where T : class
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<T> _hasher = new();
    
    public PasswordHash HashPassword(string password)
    {
        return PasswordHash.Create(_hasher.HashPassword(null!, password));
    }

    public bool VerifyHashedPassword(string password, PasswordHash hash)
    {
        return _hasher.VerifyHashedPassword(null!, hash.Hash, password) == 
               PasswordVerificationResult.Success;
    }
}