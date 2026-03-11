using DrivingSchoolApi.Application.Exceptions.Student;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class StudentService : IStudentService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IStudentRepository _studentRepository;

    public StudentService(
        IGuidGeneratorService guidGeneratorService,
        IStudentRepository studentRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _studentRepository = studentRepository;
    }
    
    public async Task<Result<Student>> CreateStudent(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        return new NotImplementedException();
    }

    public async Task<Result<Student>> GetStudentById(StudentKey id)
    {
        var student = await _studentRepository.Get(id);
        if (student is null)
            return new StudentNotFoundException();
        return student;
    }

    public async Task<Result<IEnumerable<Student>>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId)
    {
        var students = await _studentRepository.GetAll();

        return students.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result> DeleteStudent(StudentKey id)
    {
        var deleted = await _studentRepository.Delete(id);
        if (!deleted)
            return new StudentNotFoundException();
        await _studentRepository.Save();
        return Result.Success();
    }
}