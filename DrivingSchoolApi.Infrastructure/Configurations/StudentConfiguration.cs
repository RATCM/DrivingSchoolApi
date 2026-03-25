using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => StudentKey.Create(value));

        builder.OwnsOne(x => x.StudentName)
            .Property(x => x.FirstName);
        builder.OwnsOne(x => x.StudentName)
            .Property(x => x.LastName);

        builder.Property(x => x.EmailAddress).HasConversion(
            email => email.Address,
            address => Email.Create(address));
        builder
            .HasIndex(x => x.EmailAddress)
            .IsUnique();

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
            .HasForeignKey(x => x.StudentId);

        // It's a bit weird that an entity is owned but,
        // I don't really see a way around this
        builder
            .OwnsOne<StudentCalender>(x => x.Calender, calender =>
            {
                calender.WithOwner().HasForeignKey(x => x.Id);
                calender.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => StudentKey.Create(x));

                calender.OwnsMany(x => x.TimeSlots, timeSlot =>
                {
                    timeSlot.ToTable("StudentTimeSlots");
                    timeSlot.WithOwner().HasForeignKey("StudentId");
                    timeSlot.HasKey(
                         "StudentId", 
                         nameof(TimeSlot.StartDateTime),
                         nameof(TimeSlot.EndDateTime));

                     timeSlot.Property(x => x.StartDateTime);
                    timeSlot.Property(x => x.EndDateTime);
                });
            });
    }
}