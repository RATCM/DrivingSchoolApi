using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.Mappers.ValueObjectMappers;

namespace DrivingSchoolApi.Mappers;

public static class StudentMapper
{
    extension(Student entity)
    {
        public StudentDto ToDtoUnprivileged()
        {
            return new StudentDto(
                entity.Id.Value,
                entity.SchoolId.Value,
                entity.StudentName.ToDto(),
                entity.EmailAddress.ToDto(),
                entity.PhoneNumber.ToDto(),
                null,
                null
            );
        }

        //public StudentDto ToDtoPrivileged()
        //{
        //    return new StudentDto(
        //        entity.Id.Value,
        //        entity.SchoolId.Value,
        //        entity.StudentName.ToDto(),
        //        entity.EmailAddress.ToDto(),
        //        entity.PhoneNumber.ToDto(),
        //        entity.TheoryLessons.Select(t => t.ToDtoPrivileged()).ToList(),
        //        entity.DrivingLessons.Select(d => d.ToDto()).ToList()
        //    );
        //}

    }
}
