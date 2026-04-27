using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class TheoryLessonConfiguration : IEntityTypeConfiguration<TheoryLesson>
{
    public void Configure(EntityTypeBuilder<TheoryLesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => TheoryLessonKey.Create(value));


        builder.OwnsOne(x => x.Price)
            .Property(x => x.Amount);
        builder.OwnsOne(x => x.Price)
            .Property(x => x.Currency);
        
        builder.HasOne<DrivingSchool>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId);
        
        builder
            .HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(x => x.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder
            .Property<Signature>(x => x.InstructorSignature)
            .HasConversion(x => x.Blob,
                x => Signature.Create(x));
        builder
            .Property<Signature>(x => x.StudentSignature)
            .HasConversion(x => x.Blob,
                x => Signature.Create(x));
    }
}