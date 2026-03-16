using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingLessonService
{
    Task<Result<DrivingLesson>> CreateDrivingLesson(
        DrivingSchoolKey schoolId, 
        DrivingRoute route, 
        Money price, 
        InstructorKey instructorId, 
        StudentKey studentId);
    Task<Result<DrivingLesson>> GetDrivingLessonById(DrivingLessonKey id);
    Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromSchool(DrivingSchoolKey schoolId);
    Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromStudent(StudentKey studentId);
    Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromInstructor(InstructorKey instructorId);
    Task<Result> DeleteDrivingLesson(DrivingLessonKey id);
}