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
            .HasForeignKey(x => x.InstructorId);

        builder
            .HasMany<TheoryLesson>()
            .WithOne()
            .HasForeignKey(x => x.InstructorId);
        
        // It's a bit weird that an entity is owned but,
        // I don't really see a way around this
        builder
            .OwnsOne<InstructorCalender>(x => x.Calender, calender =>
            {
                calender.WithOwner().HasForeignKey(x => x.Id);
                calender.Property(x => x.Id)
                    .HasConversion(
                        x => x.Value,
                        x => InstructorKey.Create(x));

                calender.OwnsMany(x => x.TimeSlots, timeSlot =>
                {
                    timeSlot.ToTable("InstructorTimeSlots");
                    timeSlot.WithOwner().HasForeignKey("InstructorId");
                    timeSlot.HasKey(
                        "InstructorId", 
                        nameof(TimeSlot.StartDateTime),
                        nameof(TimeSlot.EndDateTime));

                    timeSlot.Property(x => x.StartDateTime);
                    timeSlot.Property(x => x.EndDateTime);
                });
            });
    }
}