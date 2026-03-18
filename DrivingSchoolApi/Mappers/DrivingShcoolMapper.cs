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
        public DrivingSchoolDto ToDto(IEnumerable<Student>? students = null, IEnumerable<Instructor>? instructors = null)
        {
            return new DrivingSchoolDto(
                entity.Id.Value,
                entity.DrivingSchoolName.ToDto(),
                entity.StreetAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                entity.WebAddress.ToDto(),
                entity.Packages.Select(x => x.ToDto()).ToList(),
                students?.Select(x=>x.ToDto()).ToList(),
                instructors?.Select(x=>x.ToDto()).ToList()
                );
        }
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
