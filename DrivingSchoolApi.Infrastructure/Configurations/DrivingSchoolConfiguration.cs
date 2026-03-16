using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrivingSchoolApi.Infrastructure.Configurations;

internal class DrivingSchoolConfiguration : IEntityTypeConfiguration<DrivingSchool>
{
    public void Configure(EntityTypeBuilder<DrivingSchool> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                key => key.Value,
                value => DrivingSchoolKey.Create(value));

        builder.Property(x => x.DrivingSchoolName)
            .HasConversion(
                name => name.Name,
                name => DrivingSchoolName.Create(name));
        
        // StreetAddress
        builder.OwnsOne(x => x.StreetAddress)
            .Property(x => x.AddressLine);
        builder.OwnsOne(x => x.StreetAddress)
            .Property(x => x.City);
        builder.OwnsOne(x => x.StreetAddress)
            .Property(x => x.PostalCode);
        builder.OwnsOne(x => x.StreetAddress)
            .Property(x => x.Region);
        
        builder.Property(x => x.PhoneNumber)
            .HasConversion(
                number => number.Number,
                number => PhoneNumber.Create(number));
        
        builder.Property(x => x.WebAddress).HasConversion(
            address => address.Url,
            url => WebAddress.Create(url));
        
        builder
            .HasMany<DrivingLesson>()
            .WithOne()
            .HasForeignKey(x => x.SchoolId);

        builder
            .HasMany<TheoryLesson>()
            .WithOne()
            .HasForeignKey(x => x.SchoolId);

        builder.OwnsMany(x => x.Packages, package =>
        {
            package.ToTable("SchoolPackage");
            package.WithOwner().HasForeignKey("SchoolId");
            package.HasKey("SchoolId", nameof(Package.Title));
            
            package.Property(x => x.Title);
            package.Property(x => x.Description);
            package.OwnsOne(x => x.Price)
                .Property(x => x.Currency);
            package.OwnsOne(x => x.Price)
                .Property(x => x.Amount);
        });
    }

}
