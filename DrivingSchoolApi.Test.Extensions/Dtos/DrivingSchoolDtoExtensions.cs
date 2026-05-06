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
            var address = new StreetAddressDto("4000","Roskilde","Hovedstaden","Main St 1");
            var phone = "1234";
            var web = "test.com";
            
            return new DrivingSchoolRegistryDto(
                name,
                address,
                phone,
                web);
        }
    }
}