using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class StudentRepository : IStudentRepository
{
    public async Task<bool> Create(Student student)
    {
        throw new NotImplementedException();
    }

    public async Task<Student?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Student>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(Student student)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}