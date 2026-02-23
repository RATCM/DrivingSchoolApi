using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class DrivingSchoolRepository : IDrivingSchoolRepository
{
    public async Task<bool> Create(DrivingSchool drivingSchool)
    {
        throw new NotImplementedException();
    }

    public async Task<DrivingSchool?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DrivingSchool>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(DrivingSchool drivingSchool)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}