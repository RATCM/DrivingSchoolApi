using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers.ValueObject;

public sealed class CoordinatePointFaker : Faker<CoordinatePoint>
{
    private int _order = 1;
    private CoordinatePointFaker() {}
    
    public static CoordinatePointFaker Create(int seed)
    {
        var faker = new CoordinatePointFaker();

        faker
            .UseSeed(seed)
            .CustomInstantiator(f => CoordinatePoint.Create(
                faker._order++,
                f.Random.Float(-180, 180),
                f.Random.Float(-180, 180)
            ));

        return faker;
    }

    public override List<CoordinatePoint> Generate(int count, string? ruleSets = null)
    {
        var coordinates = base.Generate(count, ruleSets);
        _order = 1;
        return coordinates;
    }
}