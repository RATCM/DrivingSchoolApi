using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Exceptions.Admin;

public class AdminNotFoundException : NotFoundException
{
    public AdminNotFoundException(AdminKey id) : base($"Admin with id {id.Value} was not found.") { }
}