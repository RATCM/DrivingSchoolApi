using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.StudentName)
            .Property(x => x.FirstName);
        builder.OwnsOne(x => x.StudentName)
            .Property(x => x.LastName);

        builder.Property(x => x.EmailAddress).HasConversion(
            email => email.Address,
            address => new Email(address));

        builder.Property(x => x.PhoneNumber).HasConversion(
            number => number.Number,
            number => new PhoneNumber(number));
        builder.Property(x => x.HashedPassword).HasConversion(
            x => x.Hash,
            x => new PasswordHash(x));

        builder
            .HasOne<DrivingSchool>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasMany(x => x.DrivingLessons)
            .WithOne()
            .HasForeignKey(x => x.StudentId);

        builder
            .HasMany(x => x.TheoryLessons)
            .WithMany(x => x.Students);
    }
}