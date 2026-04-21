using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class CompletedCourseConfiguration : IEntityTypeConfiguration<CompletedCourse>
{
    public void Configure(EntityTypeBuilder<CompletedCourse> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => CompletedCourseKey.Create(value));
        
        builder.OwnsOne(x => x.Cost)
            .Property(x => x.Amount);
        builder.OwnsOne(x => x.Cost)
            .Property(x => x.Currency);

        builder.Property(x => x.Reason);
        builder.Property(x => x.CompletionDate);

        builder
            .HasOne<DrivingSchool>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}