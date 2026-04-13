using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Student;

namespace DrivingSchoolApi.Mappers;

public static class StudentInviteMapper
{
    extension(StudentInvite entity)
    {
        public StudentInviteDto ToDto()
        {
            return new StudentInviteDto(entity.Id.Value);
        }
    }
}