using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingLessonService
{
    Task<DrivingLesson> CreateDrivingLesson(Guid schoolId, DrivingRoute route, Money price, Guid instructorId, Guid studentId);
    Task<DrivingLesson> GetDrivingLessonById(Guid id);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromSchool(Guid schoolId);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromStudent(Guid studentId);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromInstructor(Guid instructorId);
    Task DeleteDrivingLesson(Guid id);
}