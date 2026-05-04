using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface ITheoryLessonRepository : IRepository
{
    Task<bool> Create(TheoryLesson theoryLesson);
    Task<TheoryLesson?> Get(TheoryLessonKey id);
    Task<IEnumerable<TheoryLesson>> GetAll();
    Task<bool> Update(TheoryLesson theoryLesson);
    Task<bool> Delete(TheoryLessonKey id);
}