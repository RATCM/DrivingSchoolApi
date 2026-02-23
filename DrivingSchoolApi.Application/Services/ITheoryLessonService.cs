using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface ITheoryLessonService
{
    Task<TheoryLesson> CreateTheoryLesson(Guid schoolId, DateTime dateTime, Money price, Guid instructorId, IEnumerable<Student> students);
    Task<TheoryLesson> GetTheoryLessonById(Guid id);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromSchool(Guid schoolId);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromStudent(Guid studentId);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromInstructor(Guid instructorId);
    Task DeleteTheoryLesson(Guid id);

}