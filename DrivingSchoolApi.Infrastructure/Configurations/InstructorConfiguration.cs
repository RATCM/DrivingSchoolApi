using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => InstructorKey.Create(value));

        
        builder.OwnsOne(x => x.InstructorName)
            .Property(x => x.FirstName);
        builder.OwnsOne(x => x.InstructorName)
            .Property(x => x.LastName);

        builder.Property(x => x.EmailAddress).HasConversion(
            email => email.Address,
            address => Email.Create(address));

        builder.Property(x => x.PhoneNumber).HasConversion(
            number => number.Number,
            number => PhoneNumber.Create(number));
        builder.Property(x => x.HashedPassword).HasConversion(
            x => x.Hash,
            x => PasswordHash.Create(x));
        
        builder
            .HasOne<DrivingSchool>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasMany<DrivingLesson>()
            .WithOne()
            .HasForeignKey(x => x.InstructorId);

        builder
            .HasMany<TheoryLesson>()
            .WithOne()
            .HasForeignKey(x => x.InstructorId);
    }
}