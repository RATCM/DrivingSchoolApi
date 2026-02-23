using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.Repositories;

public interface IInstructorRepository
{
    Task<bool> Create(Instructor instructor);
    Task<Instructor?> Get(Guid id);
    Task<IEnumerable<Instructor>> GetAll();
    Task<bool> Update(Instructor instructor);
    Task<bool> Delete(Guid id);
}