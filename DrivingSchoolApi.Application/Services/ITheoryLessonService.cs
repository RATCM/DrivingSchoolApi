using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface ITheoryLessonService
{
    Task<TheoryLesson> CreateTheoryLesson(DrivingSchoolKey schoolId, 
        DateTime dateTime, 
        Money price, 
        InstructorKey instructorId, 
        IEnumerable<StudentKey> studentIds);
    Task<TheoryLesson> GetTheoryLessonById(TheoryLessonKey id);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromSchool(DrivingSchoolKey schoolId);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromStudent(StudentKey studentId);
    Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromInstructor(InstructorKey instructorId);
    Task DeleteTheoryLesson(TheoryLessonKey id);

}