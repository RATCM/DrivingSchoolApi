using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class TheoryLessonService : ITheoryLessonService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly ITheoryLessonRepository _theoryLessonRepository;

    public TheoryLessonService(
        IGuidGeneratorService guidGeneratorService, 
        ITheoryLessonRepository theoryLessonRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _theoryLessonRepository = theoryLessonRepository;
    }
    
    public async Task<TheoryLesson> CreateTheoryLesson(DrivingSchoolKey schoolId, DateTime dateTime, Money price, InstructorKey instructorId,
        IEnumerable<StudentKey> studentIds)
    {
        throw new NotImplementedException();
    }

    public async Task<TheoryLesson> GetTheoryLessonById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromStudent(StudentKey studentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TheoryLesson>> GetAllTheoryLessonsFromInstructor(InstructorKey instructorId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTheoryLesson(TheoryLessonKey id)
    {
        throw new NotImplementedException();
    }
}