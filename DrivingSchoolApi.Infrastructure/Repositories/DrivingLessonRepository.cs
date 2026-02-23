using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class DrivingLessonRepository : IDrivingLessonRepository
{
    public async Task<bool> Create(DrivingLesson drivingLesson)
    {
        throw new NotImplementedException();
    }

    public async Task<DrivingLesson?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(DrivingLesson drivingLesson)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}