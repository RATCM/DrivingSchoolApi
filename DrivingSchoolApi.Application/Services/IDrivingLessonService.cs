using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingLessonService
{
    Task<DrivingLesson> CreateDrivingLesson(
        DrivingSchoolKey schoolId, 
        DrivingRoute route, 
        Money price, 
        InstructorKey instructorId, 
        StudentKey studentId);
    Task<DrivingLesson> GetDrivingLessonById(DrivingLessonKey id);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromSchool(DrivingSchoolKey schoolId);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromStudent(StudentKey studentId);
    Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromInstructor(InstructorKey instructorId);
    Task DeleteDrivingLesson(DrivingLessonKey id);
}