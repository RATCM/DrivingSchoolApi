using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface ICompletedCourseService
{
    Task<Result<CompletedCourse>> CreateCompletedCourseForStudent(
        StudentKey studentId,
        DateTime includeCoursesFromDate,
        CourseCompletionReason reason);
    Task<Result<CompletedCourse>> GetCompletedCourseById(CompletedCourseKey id);
    Task<Result<List<CompletedCourse>>> GetAllCompletedCoursesFromStudent(StudentKey studentId);
    Task<Result<List<CompletedCourse>>> GetAllCompletedCoursesFromSchool(DrivingSchoolKey schoolId);
    Task<Result> DeleteCompletedCourse(CompletedCourseKey id);
}