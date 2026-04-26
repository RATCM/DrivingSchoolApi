using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class PackageFaker : Faker<Package>
{
    private PackageFaker(int seed)
    {
        UseSeed(seed)
            .CustomInstantiator(f => Package.Create(
                f.Commerce.ProductName(),
                f.Lorem.Text(),
                Money.Create(f.Random.Decimal(1000, 20000), f.Finance.Currency().Code)));

    }

    public static PackageFaker Create(int seed)
    {
        return new PackageFaker(seed);
    }
}