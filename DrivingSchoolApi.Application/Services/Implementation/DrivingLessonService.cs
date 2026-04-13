using DrivingSchoolApi.Application.Exceptions.DrivingLesson;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
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
    
    public async Task<Result<DrivingLesson>> CreateDrivingLesson(
        byte[] instructorSignature,
        byte[] studentSignature,
        DrivingSchoolKey schoolId, 
        DrivingRoute route, 
        Money price, 
        InstructorKey instructorId,
        StudentKey studentId)
    {
        var lesson = DrivingLesson.Create(
            DrivingLessonKey.Create(_guidGeneratorService.NewGuid()),
            schoolId,
            route,
            price,
            instructorId,
            studentId,
            Signature.Create(instructorSignature),
            Signature.Create(studentSignature));

        var created = await _drivingLessonRepository.Create(lesson);

        if (!created)
            return new Exception("Unable to create driving lesson");

        await _drivingLessonRepository.Save();
        return lesson;
    }

    public async Task<Result<DrivingLesson>> GetDrivingLessonById(DrivingLessonKey id)
    {
        var drivingLesson = await _drivingLessonRepository.Get(id);
        if(drivingLesson is null)
            return new DrivingLessonNotFoundException("Error fetching driving lesson from DB.");
        return drivingLesson;
    }

    public async Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        return lessons.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromStudent(StudentKey studentId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        return lessons.Where(x => x.StudentId.Equals(studentId)).ToList();
    }

    public async Task<Result<IEnumerable<DrivingLesson>>> GetAllDrivingLessonsFromInstructor(InstructorKey instructorId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        return lessons.Where(x => x.InstructorId.Equals(instructorId)).ToList();
    }

    public async Task<Result> DeleteDrivingLesson(DrivingLessonKey id)
    {
        var deleted = await _drivingLessonRepository.Delete(id);
        if (!deleted)
            return new DrivingLessonNotFoundException("Error deleting driving lesson.");
        await _drivingLessonRepository.Save();
        return Result.Success();
    }
}
