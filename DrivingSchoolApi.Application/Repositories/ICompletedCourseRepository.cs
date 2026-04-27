using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface ICompletedCourseRepository
{
    Task<bool> Create(CompletedCourse course);
    Task<CompletedCourse?> Get(CompletedCourseKey id);
    Task<IEnumerable<CompletedCourse>> GetAll();
    Task<bool> Delete(CompletedCourseKey id);
    Task Save();
}