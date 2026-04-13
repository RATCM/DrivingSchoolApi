using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.DTOs;
using DrivingSchoolApi.DTOs.Admin;

namespace DrivingSchoolApi.Mappers;

public static class AdminMappers
{
    extension(Admin entity)
    {
        public AdminDto ToDto()
        {
            return new AdminDto(entity.Id.Value, entity.EmailAddress.Address);
        }
    }
}