using DrivingSchoolApi.Application.Exceptions.Admin;
using DrivingSchoolApi.Application.Exceptions.Common;
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
    private readonly IAdminRepository _adminRepository;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IPasswordHasher<Student> _passwordHasherService;

    public StudentService(
        IGuidGeneratorService guidGeneratorService,
        IStudentRepository studentRepository,
        IAdminRepository adminRepository,
        ITokenGeneratorService tokenGeneratorService,
        IPasswordHasher<Student> passwordHasher)
    {
        _guidGeneratorService = guidGeneratorService;
        _studentRepository = studentRepository;
        _adminRepository = adminRepository;
        _tokenGeneratorService = tokenGeneratorService;
        _passwordHasherService = passwordHasher;
    }
    
        public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsStudent(string email, string password)
    {
        var instructor = await _studentRepository.GetByEmail(Email.Create(email));
        if (instructor is null)
            return new StudentNotFoundException("Student not found during login attempt.");

        if (!_passwordHasherService.VerifyHashedPassword(password, instructor.HashedPassword))
            return new InvalidLoginRequestException();
        
        var accessToken = _tokenGeneratorService.GenerateJwtAccessToken(instructor.Id.Value, "Student");
        var refreshToken = _tokenGeneratorService.GenerateJwtRefreshToken(instructor.Id.Value, "Student");
        return (accessToken, refreshToken);
    }
    
    public async Task<Result<Student>> CreateStudent(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        Student student = Student.Create(
            StudentKey.Create(_guidGeneratorService.NewGuid()),
            schoolId,
            name,
            email,
            _passwordHasherService.HashPassword(password),
            phoneNumber
        );

        var created = await _studentRepository.Create(student);

        if (!created)
            return new Exception("Unable to create new student");
        
        return student;
    }

    public async Task<Result<Student>> GetStudentById(StudentKey id)
    {
        var student = await _studentRepository.Get(id);
        if (student is null)
            return new StudentNotFoundException("Student not found.");
        return student;
    }

    public async Task<Result<IEnumerable<Student>>> GetAllStudents()
    {
        var students = await _studentRepository.GetAll();
        return students.ToList();
    }
    
    public async Task<Result<IEnumerable<Student>>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId)
    {
        var students = await _studentRepository.GetAll();

        return students.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result> DeleteStudent(Guid claimedId, StudentKey deleteId, bool isAdmin)
    {
        if (isAdmin)
        {
            var resultAdmin = await _adminRepository.Get(AdminKey.Create(claimedId));
            if (resultAdmin is null)
                return new AdminNotFoundException("Couldn't find your admin account in DB.");
        }
        var searchId = isAdmin ? deleteId : StudentKey.Create(claimedId);
        
        
        var deleted = await _studentRepository.Delete(searchId);
        if (!deleted)
            return new StudentNotFoundException("Student school not found.");
        await _studentRepository.Save();
        return Result.Success();
    }
}