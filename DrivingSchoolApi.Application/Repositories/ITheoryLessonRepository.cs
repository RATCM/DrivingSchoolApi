using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.Repositories;

public interface ITheoryLessonRepository
{
    Task<bool> Create(TheoryLesson theoryLesson);
    Task<TheoryLesson?> Get(Guid id);
    Task<IEnumerable<TheoryLesson>> GetAll();
    Task<bool> Update(TheoryLesson theoryLesson);
    Task<bool> Delete(Guid id);
}