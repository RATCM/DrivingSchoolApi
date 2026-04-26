using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class TimeSlotFaker : Faker<TimeSlot>
{
    private TimeSlotFaker(int seed)
    {
        UseSeed(seed)
            .CustomInstantiator(f =>
            {
                var startDateTime = f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));
                var endDateTime = f.Date.Soon(refDate: startDateTime);
                return TimeSlot.Create(
                    f.Lorem.Text(),
                    DateTimeRange.Create(
                        startDateTime, endDateTime
                    ));
            });
    }

    public static TimeSlotFaker Create(int seed)
    {
        return new TimeSlotFaker(seed);
    }
}