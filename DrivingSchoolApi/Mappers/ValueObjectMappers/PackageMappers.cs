using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.Mappers.ValueObjectMappers;

public static class PackageMappers
{
    extension(Package entity)
    {
        public PackageDto ToDto()
        {
            return new PackageDto(
                entity.Title,
                entity.Description,
                entity.Price.ToDto()
            );
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