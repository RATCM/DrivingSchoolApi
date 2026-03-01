using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Domain.Entities;
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
                entity.PackagePrice.ToDto(),
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
    
}
