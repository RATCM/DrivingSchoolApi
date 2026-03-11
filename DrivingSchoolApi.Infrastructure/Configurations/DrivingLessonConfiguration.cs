using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class DrivingLessonConfiguration : IEntityTypeConfiguration<DrivingLesson>
{
    public void Configure(EntityTypeBuilder<DrivingLesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => DrivingLessonKey.Create(value));


        builder.OwnsOne(x => x.Route, route =>
        {
            route.OwnsOne(x => x.DateTimeRange)
                .Property(x => x.StartDateTime);
            route.OwnsOne(x => x.DateTimeRange)
                .Property(x => x.EndDateTime);

            route.OwnsMany(x => x.RouteCoordinates, coordinate =>
            {
                coordinate.WithOwner().HasForeignKey("RouteId");
                coordinate.HasKey("RouteId", nameof(CoordinatePoint.Order));
                
                coordinate.Property(x => x.Order)
                    .IsRequired();
                coordinate.Property(x => x.Latitude)
                    .IsRequired();
                coordinate.Property(x => x.Longitude)
                    .IsRequired();
            });
        });

        
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
    }
}