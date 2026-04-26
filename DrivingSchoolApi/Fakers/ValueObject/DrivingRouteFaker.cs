using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers.ValueObject;

public sealed class DrivingRouteFaker : Faker<DrivingRoute>
{
    private DrivingRouteFaker() {}

    public static DrivingRouteFaker Create(int seed)
    {
        var coordinatePointFaker = CoordinatePointFaker.Create(seed);
        var dateTimeRangeFaker = DateTimeRangeFaker.Create(seed);
        var faker = new DrivingRouteFaker();
        faker
            .UseSeed(seed)
            .CustomInstantiator(f =>
            {
                int numCoordinates = f.Random.Number(20, 100);
                var coordinatePoints = coordinatePointFaker.Generate(numCoordinates);
                
                return DrivingRoute.Create(
                    dateTimeRangeFaker.Generate(),
                    coordinatePoints.ToArray());
            });

        return faker;
    }
}