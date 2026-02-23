using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class InstructorRepository : IInstructorRepository
{
    public async Task<bool> Create(Instructor instructor)
    {
        throw new NotImplementedException();
    }

    public async Task<Instructor?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Instructor>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(Instructor instructor)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}