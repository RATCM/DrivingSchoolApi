using DrivingSchoolApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class TheoryLessonConfiguration : IEntityTypeConfiguration<TheoryLesson>
{
    public void Configure(EntityTypeBuilder<TheoryLesson> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Price)
            .Property(x => x.Amount);
        builder.OwnsOne(x => x.Price)
            .Property(x => x.Currency);
        
        builder.HasOne<DrivingSchool>()
            .WithMany(x => x.TheoryLessons)
            .HasForeignKey(x => x.SchoolId);
        
        builder.HasOne<Instructor>()
            .WithMany(x => x.TheoryLessons)
            .HasForeignKey(x => x.InstructorId);

        builder
            .HasMany(x => x.Students)
            .WithMany(x => x.TheoryLessons);
    }
}