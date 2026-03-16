using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class PackageMappers
{
    extension(Package entity)
    {
        public PackageDto ToDto()
        {
            return new PackageDto
            {
                Description = entity.Description,
                Title = entity.Title,
                Price = entity.Price.ToDto()
            };
        }
    }

    extension(PackageDto dto)
    {
        public Package ToDomain()
        {
            return Package.Create(
                dto.Title,
                dto.Description,
                dto.Price.ToDomain()
            );
        }
    }
}