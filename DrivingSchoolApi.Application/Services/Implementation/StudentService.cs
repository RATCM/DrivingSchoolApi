using DrivingSchoolApi.Application.Exceptions.Student;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
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
    
    public async Task<Student> CreateStudent(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task<Student> GetStudentById(StudentKey id)
    {
        return await _studentRepository.Get(id) ?? throw new StudentNotFoundException();
    }

    public async Task<IEnumerable<Student>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId)
    {
        var students = await _studentRepository.GetAll();

        return students.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task DeleteStudent(StudentKey id)
    {
        var deleted = await _studentRepository.Delete(id);
        if (!deleted)
            throw new StudentNotFoundException();
        await _studentRepository.Save();
    }
}