using DrivingSchoolApi.Application.Exceptions.Instructor;
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
    private readonly IInstructorRepository _instructorRepository;

    public TheoryLessonService(
        IGuidGeneratorService guidGeneratorService, 
        ITheoryLessonRepository theoryLessonRepository,
        IInstructorRepository instructorRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _theoryLessonRepository = theoryLessonRepository;
        _instructorRepository = instructorRepository;
    }
    
    public async Task<Result<TheoryLesson>> CreateTheoryLesson(
        byte[] instructorSignature,
        byte[] studentSignature,
        InstructorKey instructorId,
        DateTime dateTime,
        Money price, 
        StudentKey studentId)
    {
        var instructor = await _instructorRepository.Get(instructorId);
        if (instructor is null)
            return new InstructorNotFoundException($"Instructor was not found.");
        
        var lesson = TheoryLesson.Create(
            TheoryLessonKey.Create(_guidGeneratorService.NewGuid()),
            instructor.SchoolId,
            dateTime,
            price,
            instructorId,
            studentId,
            Signature.Create(instructorSignature),
            Signature.Create(studentSignature));

        var created = await _theoryLessonRepository.Create(lesson);

        if (!created)
            return new Exception("Unable to create theory lesson");
        
        await  _theoryLessonRepository.Save();
        return lesson;
    }

    public async Task<Result<TheoryLesson>> GetTheoryLessonById(TheoryLessonKey id)
    {
        var result = await _theoryLessonRepository.Get(id);
        
        if (result is null)
            return new TheoryLessonNotFoundException("Theory lesson not found.");
        
        return result;
    }

    public async Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        var lessons = await _theoryLessonRepository.GetAll();

        return lessons.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result<IEnumerable<TheoryLesson>>> GetAllTheoryLessonsFromStudent(StudentKey studentId)
    {
        var lessons = await _theoryLessonRepository.GetAll();

        return lessons.Where(x => x.StudentId.Equals(studentId)).ToList();
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
            return new TheoryLessonNotFoundException("Theory lesson not found.");
        await _theoryLessonRepository.Save();
        return Result.Success();
    }
}