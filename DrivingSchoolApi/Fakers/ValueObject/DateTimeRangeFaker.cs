using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers.ValueObject;

public class DateTimeRangeFaker : Faker<DateTimeRange>
{
    private DateTimeRangeFaker() {}

    public static DateTimeRangeFaker Create(int seed, int maxDays = 1)
    {
        var faker = new DateTimeRangeFaker();

        faker
            .UseSeed(seed)
            .CustomInstantiator(f =>
            {
                var startDateTime = f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));
                var endDateTime = f.Date.Soon(days: maxDays, refDate: startDateTime);

                return DateTimeRange.Create(startDateTime, endDateTime);
            });

        return faker;
    }
}