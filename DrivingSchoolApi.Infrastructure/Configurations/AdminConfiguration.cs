using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => AdminKey.Create(value));
        
        builder.Property(x => x.EmailAddress).HasConversion(
            email => email.Address,
            address => Email.Create(address));
        
        builder.Property(x => x.HashedPassword).HasConversion(
            x => x.Hash,
            x => PasswordHash.Create(x));
    }
}