using DrivingSchoolApi.Application.Exceptions.CompletedCourse;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

public class CompletedCourseService : ICompletedCourseService
{
    private readonly IDateTimeProviderService _dateTimeProviderService;
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly ICompletedCourseRepository _completedCourseRepository;
    private readonly IStudentService _studentService;
    private readonly ITheoryLessonService _theoryLessonService;
    private readonly IDrivingLessonService _drivingLessonService;
    private readonly IDrivingSchoolService _drivingSchoolService;
    
    public CompletedCourseService(
        IDateTimeProviderService dateTimeProviderService,
        IGuidGeneratorService guidGeneratorService,
        IStudentService studentService,
        ITheoryLessonService theoryLessonService,
        IDrivingLessonService drivingLessonService,
        ICompletedCourseRepository completedCourseRepository,
        IDrivingSchoolService drivingSchoolService)
    {
        _dateTimeProviderService = dateTimeProviderService;
        _guidGeneratorService = guidGeneratorService;
        _studentService = studentService;
        _theoryLessonService = theoryLessonService;
        _drivingLessonService = drivingLessonService;
        _completedCourseRepository = completedCourseRepository;
        _drivingSchoolService = drivingSchoolService;
    }
    
    public async Task<Result<CompletedCourse>> CreateCompletedCourseForStudent(
        StudentKey studentId, 
        DateTime includeCoursesFromDate,
        CourseCompletionReason reason)
    {
        var student = await _studentService.GetStudentById(studentId);
        var theoryLessons = await _theoryLessonService.GetAllTheoryLessonsFromStudent(studentId);
        var drivingLessons = await _drivingLessonService.GetAllDrivingLessonsFromStudent(studentId);

        if (!student.IsSuccess) return student.Error!;
        if (!theoryLessons.IsSuccess) return theoryLessons.Error!;
        if (!drivingLessons.IsSuccess) return drivingLessons.Error!;

        var theoryLessonsFromDate = theoryLessons.Value!.Where(x => x.LessonDateTime >= includeCoursesFromDate);
        var drivingLessonsFromDate = drivingLessons.Value!.Where(x => x.Route.DateTimeRange.StartDateTime >= includeCoursesFromDate);

        // We should find a way to convert different currencies
        // for now we just assume everything is in DKK
        var cost = theoryLessonsFromDate.Select(x => x.Price.Amount).Sum() +
                    drivingLessonsFromDate.Select(x => x.Price.Amount).Sum();
        
        var completion = CompletedCourse.Create(
            CompletedCourseKey.Create(_guidGeneratorService.NewGuid()),
            student.Value!.SchoolId,
            studentId,
            Money.Create(cost, "DKK"),
            _dateTimeProviderService.Now(),
            reason
        );

        var created = await _completedCourseRepository.Create(completion);
        if (!created)
            return new Exception("Unable to create course completion");

        await _completedCourseRepository.Save();
        return completion;
    }

    public async Task<Result<CompletedCourse>> GetCompletedCourseById(CompletedCourseKey id)
    {
        var course = await _completedCourseRepository.Get(id);

        if (course is null)
            return new CompletedCourseNotFoundException("Completed course could not be found");

        return course;
    }

    public async Task<Result<List<CompletedCourse>>> GetAllCompletedCoursesFromStudent(StudentKey studentId)
    {
        // Check if student exists
        var student = await _studentService.GetStudentById(studentId);
        if (!student.IsSuccess) return student.Error!;
        
        var courses = await _completedCourseRepository.GetAll();

        return courses.Where(x => studentId.Equals(x.StudentId)).ToList();
    }
    
    public async Task<Result<List<CompletedCourse>>> GetAllCompletedCoursesFromSchool(DrivingSchoolKey schoolId)
    {
        // Check if driving school exists
        var school = await _drivingSchoolService.GetDrivingSchoolById(schoolId);
        if (!school.IsSuccess) return school.Error!;
        
        var courses = await _completedCourseRepository.GetAll();

        return courses.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result> DeleteCompletedCourse(CompletedCourseKey id)
    {
        var deleted = await _completedCourseRepository.Delete(id);

        if (!deleted)
            return new CompletedCourseNotFoundException("Completed course could not be found");

        await _completedCourseRepository.Save();
        return Result.Success();
    }
}