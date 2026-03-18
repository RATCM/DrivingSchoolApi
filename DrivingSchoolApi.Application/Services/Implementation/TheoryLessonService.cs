using DrivingSchoolApi.Application.Exceptions.TheoryLesson;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
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
    
    public async Task<Result<TheoryLesson>> CreateTheoryLesson(
        DrivingSchoolKey schoolId, 
        DateTime dateTime, 
        Money price, 
        InstructorKey instructorId,
        IEnumerable<StudentKey> studentIds)
    {
        var lesson = TheoryLesson.Create(
            TheoryLessonKey.Create(_guidGeneratorService.NewGuid()),
            schoolId,
            dateTime,
            price,
            instructorId, 
            studentIds);

        var created = await _theoryLessonRepository.Create(lesson);

        if (!created)
            return new Exception("Unable to create theory lesson");
        
        await  _theoryLessonRepository.Save();
        return lesson;
    }

    public async Task<Result<TheoryLesson>> GetTheoryLessonById(TheoryLessonKey id)
    {
        return await _theoryLessonRepository.Get(id) ?? throw new TheoryLessonNotFoundException();
    }

    public async Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        var lessons = await _theoryLessonRepository.GetAll();

        return lessons.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromStudent(StudentKey studentId)
    {
        var lessons = await _theoryLessonRepository.GetAll();

        return lessons.Where(x => x.StudentIds.Contains(studentId)).ToList();
    }

    public async Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromInstructor(InstructorKey instructorId)
    {
        var lessons = await _theoryLessonRepository.GetAll();

        return lessons.Where(x => x.InstructorId.Equals(instructorId)).ToList();
    }

    public async Task<Result> DeleteTheoryLesson(TheoryLessonKey id)
    {
        var deleted = await _theoryLessonRepository.Delete(id);
        if (!deleted)
            return new TheoryLessonNotFoundException();
        await _theoryLessonRepository.Save();
        return Result.Success();
    }
}