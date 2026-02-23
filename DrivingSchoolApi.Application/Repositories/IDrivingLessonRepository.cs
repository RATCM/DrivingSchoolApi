using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.Repositories;

public interface IDrivingLessonRepository
{
    Task<bool> Create(DrivingLesson drivingLesson);
    Task<DrivingLesson?> Get(Guid id);
    Task<IEnumerable<DrivingLesson>> GetAll();
    Task<bool> Update(DrivingLesson drivingLesson);
    Task<bool> Delete(Guid id);
}