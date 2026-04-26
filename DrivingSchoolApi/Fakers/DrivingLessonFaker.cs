using Bogus;
using DrivingSchoolApi.Application.Exceptions;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Fakers.ValueObject;

namespace DrivingSchoolApi.Fakers;

public sealed class DrivingLessonFaker : Faker<DrivingLesson>
{
    private DrivingLessonFaker() {}
    
    public static Result<DrivingLessonFaker> Create(
        int seed,
        IEnumerable<DrivingSchool> drivingSchools,
        IEnumerable<Instructor> instructors,
        IEnumerable<Student> students)
    {
        var drivingSchoolIds = drivingSchools.Select(x => x.Id).ToList();
        if (drivingSchoolIds.Count == 0)
            return new BadRequestException("Cannot add driving lessons if there are no driving schools");

        var instructorIds = instructors.GroupBy(x => x.SchoolId, x => x.Id).ToList();
        if(instructorIds.Count == 0)
            return new BadRequestException("Cannot add driving lessons if there are no instructors");
        
        var studentIds = students.GroupBy(x => x.SchoolId, x => x.Id).ToList();
        if(studentIds.Count == 0)
            return new BadRequestException("Cannot add driving lessons if there are no students");

        var routeFaker = DrivingRouteFaker.Create(seed);
        var signatureFaker = SignatureFaker.Create(seed);

        var faker = new DrivingLessonFaker();
        faker
            .UseSeed(seed)
            .CustomInstantiator(f =>
            {
                // Generate route
                var schoolId = f.PickRandom(drivingSchoolIds);

                return DrivingLesson.Create(
                    DrivingLessonKey.Create(Guid.NewGuid()),
                    schoolId,
                    routeFaker.Generate(),
                    Money.Create(f.Random.Decimal(1000, 2000), f.Finance.Currency().Code),
                    f.PickRandom(instructorIds.First(grouping => grouping.Key.Equals(schoolId)).ToList()),
                    f.PickRandom(studentIds.First(grouping => grouping.Key.Equals(schoolId)).ToList()),
                    signatureFaker.Generate(),
                    signatureFaker.Generate()
                );
            });

        return faker;
    }
}