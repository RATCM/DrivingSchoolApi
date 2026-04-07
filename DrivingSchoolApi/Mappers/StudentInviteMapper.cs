using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;

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