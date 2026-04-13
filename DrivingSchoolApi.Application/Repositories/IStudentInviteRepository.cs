using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface IStudentInviteRepository
{
    Task<StudentInvite?> Get(StudentInviteKey id);
    Task<bool> Delete(StudentInviteKey id);
    Task Save();
}