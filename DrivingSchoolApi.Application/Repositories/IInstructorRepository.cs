using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Repositories;

public interface IInstructorRepository
{
    Task<bool> Create(Instructor instructor);
    Task<Instructor?> Get(InstructorKey id);
    Task<Instructor?> GetByEmail(Email email);
    Task<IEnumerable<Instructor>> GetAll();
    Task<bool> Update(Instructor instructor);
    Task<bool> Delete(InstructorKey id);
    Task Save();
}