using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface IDrivingLessonRepository
{
    Task<bool> Create(DrivingLesson drivingLesson);
    Task<DrivingLesson?> Get(DrivingLessonKey id);
    Task<IEnumerable<DrivingLesson>> GetAll();
    Task<bool> Update(DrivingLesson drivingLesson);
    Task<bool> Delete(DrivingLessonKey id);
    Task Save();
}