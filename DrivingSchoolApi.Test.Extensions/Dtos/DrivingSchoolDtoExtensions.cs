using DrivingSchoolApi.DTOs.DrivingSchool;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class DrivingSchoolDtoExtensions
{
    extension(DrivingSchoolRegistryDto registryDto)
    {
        public static DrivingSchoolRegistryDto CreateTestSchool()
        {
            var name = "Test School";
            var address = new StreetAddressDto("4000","Test City","Test Region","Test Street");
            var phone = "12345678";
            var web = "test.com";
            
            return new DrivingSchoolRegistryDto(
                name,
                address,
                phone,
                web);
        }
    }
}