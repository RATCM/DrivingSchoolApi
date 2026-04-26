using Bogus;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class StudentFaker : Faker<Student>
{
    private string? _password = null;
    private StudentFaker(int seed, 
        IEnumerable<DrivingSchoolKey> drivingSchoolIds, 
        IPasswordHasher<Student> studentPasswordHasher)
    {
        var timeSlotFaker = TimeSlotFaker.Create(seed);
        
        UseSeed(seed)
            .CustomInstantiator(f =>
            {
                var student = Student.Create(
                    StudentKey.Create(Guid.NewGuid()),
                    f.PickRandom(drivingSchoolIds),
                    Name.Create(f.Person.FirstName, f.Person.LastName),
                    Email.Create(f.Person.Email),
                    studentPasswordHasher.HashPassword(_password ?? f.Random.AlphaNumeric(16)),
                    PhoneNumber.Create(f.Random.String(8, minChar: '0', maxChar: '9')));

                var timeSlots = timeSlotFaker.Generate(f.Random.Number(min: 1, max: 20)) ?? [];
                timeSlots.ForEach(x => student.Calender.AddTimeSlot(x));
                return student;
            });
    }

    public StudentFaker UsePassword(string? password)
    {
        _password = password;

        return this;
    }

    public static StudentFaker Create(
        int seed,
        IEnumerable<DrivingSchoolKey> drivingSchoolIds,
        IPasswordHasher<Student> studentPasswordHasher)
    {
        return new StudentFaker(seed, drivingSchoolIds, studentPasswordHasher);
    }
}