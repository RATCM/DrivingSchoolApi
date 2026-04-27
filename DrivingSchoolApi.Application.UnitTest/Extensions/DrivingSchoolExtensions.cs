using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class DrivingSchoolExtensions
{
    extension(DrivingSchool drivingSchool)
    {
        public static DrivingSchool CreateTestSchool(Guid? schoolId = null)
        {
            schoolId ??= Guid.NewGuid();
            
            var name = DrivingSchoolName.Create("Test School");
            var address = StreetAddress.Create("12345", "City", "Region", "Main St 1");
            var phone = PhoneNumber.Create("1234");
            var web = WebAddress.Create("test.com");
            var packages = Array.Empty<Package>();
            
            return DrivingSchool.Create(
                DrivingSchoolKey.Create(schoolId.Value),
                name,
                address,
                phone,
                web,
                packages);
        }
    }
}
