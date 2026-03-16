using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class PackageMappers
{
    extension(Package value)
    {
        public PackageDto ToDto()
        {
            return new PackageDto
            {
                Description = value.Description,
                Title = value.Title,
                Price = value.Price.ToDto()
            };
        }
    }
}