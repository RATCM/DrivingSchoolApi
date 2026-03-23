using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Exceptions.Admin;

public class AdminNotFoundException : NotFoundException
{
    public AdminNotFoundException(string message) : base(message) { }
}