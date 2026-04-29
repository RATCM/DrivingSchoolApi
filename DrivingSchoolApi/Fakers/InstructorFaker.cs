using Bogus;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class InstructorFaker : Faker<Instructor>
{
    private string? _password = null;
    private InstructorKey? _id = null;

    private InstructorFaker(int seed, 
        IEnumerable<DrivingSchoolKey> drivingSchoolIds,
        IPasswordHasher<Instructor> instructorPasswordHasher)
    {
        var timeSlotFaker = TimeSlotFaker.Create(seed);

        UseSeed(seed)
            .CustomInstantiator(f =>
                {
                    var instructor = Instructor.Create(
                        _id ?? InstructorKey.Create(Guid.NewGuid()),
                        f.PickRandom(drivingSchoolIds),
                        Name.Create(f.Person.FirstName, f.Person.LastName),
                        Email.Create(f.Person.Email),
                        instructorPasswordHasher.HashPassword(_password ?? f.Random.AlphaNumeric(16)),
                        PhoneNumber.Create(f.Random.String(8, minChar: '0', maxChar: '9'))
                    );

                    var timeSlots = timeSlotFaker.Generate(f.Random.Number(min: 1, max: 20)) ?? [];
                    timeSlots.ForEach(x => instructor.Calender.AddTimeSlot(x));
                    return instructor;
                }
            );
    }

    public InstructorFaker UsePassword(string? password)
    {
        _password = password;

        return this;
    }
    
    public InstructorFaker UseId(InstructorKey? id)
    {
        _id = id;

        return this;
    }
    
    public static InstructorFaker Create(int seed,
        IEnumerable<DrivingSchoolKey> drivingSchoolIds,
        IPasswordHasher<Instructor> instructorPasswordHasher)
    {
        return new InstructorFaker(seed, drivingSchoolIds, instructorPasswordHasher);
    }
}