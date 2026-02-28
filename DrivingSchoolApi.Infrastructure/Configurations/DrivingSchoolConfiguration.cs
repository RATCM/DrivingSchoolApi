using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class DrivingSchoolConfiguration : IEntityTypeConfiguration<DrivingSchool>
{
    public void Configure(EntityTypeBuilder<DrivingSchool> builder)
    {
        builder.HasKey(x => x.Id);

        // StreetAddress
        builder.OwnsOne(x => x.SchoolStreetAddress)
            .Property(x => x.AddressLine);
        builder.OwnsOne(x => x.SchoolStreetAddress)
            .Property(x => x.City);
        builder.OwnsOne(x => x.SchoolStreetAddress)
            .Property(x => x.PostalCode);
        builder.OwnsOne(x => x.SchoolStreetAddress)
            .Property(x => x.Region);
        
        builder.Property(x => x.PhoneNumber)
            .HasConversion(
                number => number.Number,
                number => new PhoneNumber(number));
        
        builder.Property(x => x.WebAddress).HasConversion(
            address => address.Url,
            url => new WebAddress(url));

        builder
            .HasMany(x => x.Students)
            .WithOne()
            .HasForeignKey(x => x.SchoolId);
        
        builder
            .HasMany(x => x.Instructors)
            .WithOne()
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasMany<DrivingLesson>()
            .WithOne()
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasMany<TheoryLesson>()
            .WithOne()
            .HasForeignKey(x => x.SchoolId);

    }
}