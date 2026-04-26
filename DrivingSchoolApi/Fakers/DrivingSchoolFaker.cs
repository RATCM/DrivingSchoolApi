using Bogus;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class DrivingSchoolFaker : Faker<DrivingSchool>
{
    private DrivingSchoolFaker(int seed)
    {
        var packageFaker = PackageFaker.Create(seed);
        
        UseSeed(seed)
            .CustomInstantiator(f =>
            {
                var school = DrivingSchool.Create(
                    DrivingSchoolKey.Create(Guid.NewGuid()),
                    DrivingSchoolName.Create(f.Company.CompanyName()),
                    StreetAddress.Create(
                        f.Address.ZipCode(),
                        f.Address.City(),
                        f.Address.State(),
                        f.Address.StreetAddress()
                    ),
                    PhoneNumber.Create(f.Phone.PhoneNumber()),
                    WebAddress.Create(f.Internet.Url()),
                    packageFaker.Generate(f.Random.Number(1, 20))?.ToArray() ?? []
                );

                // Add student invites
                var amount = f.Random.Number(1, 20);
                for (int i = 0; i < amount; i++)
                {
                    school.AddStudentInvite(
                        StudentInvite.Create(
                            StudentInviteKey.Create(Guid.NewGuid()),
                            school.Id,
                            f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1))));
                }

                return school;
            });
    }

    public static DrivingSchoolFaker Create(int seed)
    {
        return new DrivingSchoolFaker(seed);
    }
}