using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class DrivingLessonService : IDrivingLessonService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IDrivingLessonRepository _drivingLessonRepository;

    public DrivingLessonService(
        IGuidGeneratorService guidGeneratorService,
        IDrivingLessonRepository drivingLessonRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _drivingLessonRepository = drivingLessonRepository;
    }
    
    public async Task<DrivingLesson> CreateDrivingLesson(DrivingSchoolKey schoolId, DrivingRoute route, Money price, InstructorKey instructorId,
        StudentKey studentId)
    {
        throw new NotImplementedException();
    }

    public async Task<DrivingLesson> GetDrivingLessonById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromStudent(StudentKey studentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromInstructor(InstructorKey instructorId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDrivingLesson(DrivingLessonKey id)
    {
        throw new NotImplementedException();
    }
}