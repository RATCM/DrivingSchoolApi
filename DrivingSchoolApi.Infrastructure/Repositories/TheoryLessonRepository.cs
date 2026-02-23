using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class TheoryLessonRepository : ITheoryLessonRepository
{
    public async Task<bool> Create(TheoryLesson theoryLesson)
    {
        throw new NotImplementedException();
    }

    public async Task<TheoryLesson?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TheoryLesson>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(TheoryLesson theoryLesson)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}