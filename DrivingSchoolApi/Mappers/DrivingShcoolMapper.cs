using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class DrivingShcoolMapper
{
    extension(DrivingSchool entity)
    {
        public DrivingSchoolDto ToDtoUnprivileged()
        {
            return new DrivingSchoolDto(
                entity.Id.Value,
                entity.DrivingSchoolName.ToDto(),
                entity.StreetAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                entity.WebAddress.ToDto(),
                entity.Packages.Select(x => x.ToDto()).ToList(),
                null,
                null
                );
        }
        
        //public DrivingSchoolDto ToDtoPrivileged()
        //{
        //    return new DrivingSchoolDto(
        //        entity.Id.Value,
        //        entity.DrivingSchoolName.ToDto(),
        //        entity.StreetAddress.ToDto(),
        //        entity.PhoneNumber.ToDto(),
        //        entity.WebAddress.ToDto(),
        //        entity.PackagePrice.ToDto(),
        //        entity.Students.Select(s => s.ToDto()).ToList(),
        //        entity.Instructors.Select(i => i.ToDto()).ToList()
        //    );
        //}
        
    }

    extension(DrivingSchoolDto dto)
    {
        public DrivingSchool ToDomain()
        {
            return DrivingSchool.Create(
                DrivingSchoolKey.Create(dto.Id), 
                DrivingSchoolName.Create(dto.Name),
                dto.StreetAddress.ToDomain(),
                PhoneNumber.Create(dto.PhoneNumber),
                WebAddress.Create(dto.WebAddress),
                dto.Packages.Select(x => x.ToDomain()).ToArray());
            
        }
    }
}
