using DrivingSchoolApi.Application.Exceptions.Instructor;
using DrivingSchoolApi.Application.Exceptions.Student;
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
    private readonly IStudentRepository _studentRepository;
    private readonly IInstructorRepository _instructorRepository;

    public TheoryLessonService(
        IGuidGeneratorService guidGeneratorService, 
        ITheoryLessonRepository theoryLessonRepository,
        IStudentRepository studentRepository,
        IInstructorRepository instructorRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _theoryLessonRepository = theoryLessonRepository;
        _studentRepository = studentRepository;
        _instructorRepository = instructorRepository;
    }
    
    public async Task<Result<TheoryLesson>> CreateTheoryLesson(
        InstructorKey instructorId,
        DateTime dateTime,
        Money price, 
        IEnumerable<StudentKey> studentIds,
        byte[] instructorSignature)
    {
        // Materialize studentIds once to avoid multiple enumeration
        var studentIdsList = studentIds.ToList();
        
        // No duplicates
        if (studentIdsList.Count != studentIdsList.Distinct().Count())
            return new StudentDuplicateException("Cannot add duplicate students to theory lesson.");
        
        var instructor = await _instructorRepository.Get(instructorId);
        if (instructor is null)
            return new InstructorNotFoundException($"Instructor was not found.");
        
        // Build allowed student id set for the instructor's school.
        var schoolStudents = await _studentRepository.GetAllFromDrivingSchool(instructor.SchoolId);
        var allowedStudentIds = schoolStudents.Select(x => x.Id).ToHashSet();
        
        // Validate all requested students are from the same school as the instructor.
        var invalidStudentIds = studentIdsList
            .Where(id => !allowedStudentIds.Contains(id))
            .ToList();
        
        if (invalidStudentIds.Count != 0)
            return new Exception("One or more students are not in the instructor's school.");
        
        var lesson = TheoryLesson.Create(
            TheoryLessonKey.Create(_guidGeneratorService.NewGuid()),
            instructor.SchoolId,
            dateTime,
            price,
            instructorId, 
            studentIdsList,
            Signature.Create(instructorSignature));

        var created = await _theoryLessonRepository.Create(lesson);

        if (!created)
            return new Exception("Unable to create theory lesson");
        
        await  _theoryLessonRepository.Save();
        return lesson;
    }

    public async Task<Result<TheoryLesson>> GetTheoryLessonById(TheoryLessonKey id)
    {
        return await _theoryLessonRepository.Get(id) ?? throw new TheoryLessonNotFoundException("Theory lesson not found.");
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
            return new TheoryLessonNotFoundException("Theory lesson not found.");
        await _theoryLessonRepository.Save();
        return Result.Success();
    }
}