using DrivingSchoolApi.Application.Exceptions.DrivingLesson;
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
    
    public async Task<DrivingLesson> CreateDrivingLesson(
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
            studentId);

        var created = await _drivingLessonRepository.Create(lesson);

        if (!created)
            throw new Exception("Unable to create driving lesson");

        await _drivingLessonRepository.Save();
        return lesson;
    }

    public async Task<DrivingLesson> GetDrivingLessonById(DrivingLessonKey id)
    {
        return await _drivingLessonRepository.Get(id) ?? throw new DrivingLessonNotFoundException();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromSchool(DrivingSchoolKey schoolId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        
        return lessons.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromStudent(StudentKey studentId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        
        return lessons.Where(x => x.StudentId.Equals(studentId)).ToList();
    }

    public async Task<IEnumerable<DrivingLesson>> GetAllDrivingLessonsFromInstructor(InstructorKey instructorId)
    {
        var lessons = await _drivingLessonRepository.GetAll();
        
        return lessons.Where(x => x.InstructorId.Equals(instructorId)).ToList();
    }

    public async Task DeleteDrivingLesson(DrivingLessonKey id)
    {
        var deleted = await _drivingLessonRepository.Delete(id);
        if (!deleted)
            throw new DrivingLessonNotFoundException();
        await _drivingLessonRepository.Save();
    }
}