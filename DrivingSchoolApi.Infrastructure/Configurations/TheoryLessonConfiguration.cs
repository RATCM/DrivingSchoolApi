using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
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
            .HasForeignKey(x => x.InstructorId);
        
        // Configures many-to-many relationship with students
        builder.OwnsMany(a => a.StudentIds, b =>
        {
            b.ToTable("StudentTheoryLesson");
            b.WithOwner().HasForeignKey("TheoryLessonId");
            
            b.Property<StudentKey>("StudentId")
                .HasConversion(
                    x => x.Value,
                    x => StudentKey.Create(x));
            
            b.HasKey("TheoryLessonId", "StudentId");

            b.HasOne<Student>()
                .WithMany()
                .HasForeignKey("StudentId");
        });
    }
}