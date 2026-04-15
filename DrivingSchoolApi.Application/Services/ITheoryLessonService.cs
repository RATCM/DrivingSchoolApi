using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface ITheoryLessonService
{
    Task<Result<TheoryLesson>> CreateTheoryLesson(
        InstructorKey instructorId,
        DateTime dateTime, 
        Money price,
        IEnumerable<StudentKey> studentIds,
        byte[] instructorSignature);
    Task<Result<TheoryLesson>> GetTheoryLessonById(TheoryLessonKey id);
    Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromSchool(DrivingSchoolKey schoolId);
    Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromStudent(StudentKey studentId);
    Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromInstructor(InstructorKey instructorId);
    Task<Result> DeleteTheoryLesson(TheoryLessonKey id);

}