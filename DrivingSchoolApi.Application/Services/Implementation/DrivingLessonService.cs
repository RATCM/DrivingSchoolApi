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
        DrivingSchoolKey schoolId, 
        DrivingRoute route, 
        Money price, 
        InstructorKey instructorId,
        StudentKey studentId)
    {
        //TODO validations
        
        //var instructorResult = await _instructorService.GetInstructorById(InstructorKey.Create(idClaim));
        //if (!instructorResult.IsSuccess) { return BadRequest("Failed to retrieve instructor."); }
        //var instructor = instructorResult.Value!;
        //
        //var studentResult = await _studentService.GetStudentById(StudentKey.Create(registryDto.StudentId));
        //if (!studentResult.IsSuccess) { return BadRequest("Failed to retrieve student."); }
        //var student = studentResult.Value!;
        //
//
        //// Check that student is from the same school as instructor
        //if (student.SchoolId.Value != instructor.SchoolId.Value)
        //{
        //    return BadRequest("Student is not assigned to the same school as the instructor.");
        //}
        //
        ////TODO check that information is correct (e.g. price is not negative, route is valid etc.)
        
        var lesson = DrivingLesson.Create(
            DrivingLessonKey.Create(_guidGeneratorService.NewGuid()),
            schoolId,
            route,
            price,
            instructorId,
            studentId);

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