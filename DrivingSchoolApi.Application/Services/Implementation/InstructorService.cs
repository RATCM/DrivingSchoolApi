using DrivingSchoolApi.Application.Exceptions.Instructor;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class InstructorService : IInstructorService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IInstructorRepository _instructorRepository;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IPasswordHasher<Instructor> _passwordHasherService;

    public InstructorService(
        IGuidGeneratorService guidGeneratorService,
        IInstructorRepository instructorRepository,
        ITokenGeneratorService tokenGeneratorService,
        IPasswordHasher<Instructor> passwordHasher)
    {
        _guidGeneratorService = guidGeneratorService;
        _instructorRepository = instructorRepository;
        _tokenGeneratorService = tokenGeneratorService;
        _passwordHasherService = passwordHasher;
    }

    public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsInstructor(string email, string password)
    {
        //var instructor = await _instructorRepository.Get(instructorId);
        //
        //var accessToken = _tokenGeneratorService.GenerateJwtAccessToken(instructorId.Value, "Instructor");
        //var refreshToken = _tokenGeneratorService.GenerateJwtRefreshToken(instructorId.Value, "Instructor");
        //return (accessToken, refreshToken);
        return new NotImplementedException();
    }
    
    public async Task<Result<Instructor>> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        return new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructors()
    {
        var instructors = await _instructorRepository.GetAll();
        return instructors.ToList();
    }

    public async Task<Result<Instructor>> GetInstructorById(InstructorKey id) 
    {
        var instructor = await _instructorRepository.Get(id);
        if(instructor is null)
            return new InstructorNotFoundException();
        return instructor;
    }

    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId)
    {
        var instructors = await _instructorRepository.GetAll();
        
        return instructors.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task<Result<Instructor>> UpdateInstructor(InstructorKey id, DrivingSchoolKey schoolId, Name name, Email email, PhoneNumber phoneNumber)
    {
        return new NotImplementedException();
    }

    public async Task<Result<Instructor>> UpdateInstructorPassword(InstructorKey id, string oldPassword, string newPassword)
    {
        return new NotImplementedException();
    }
    
    
    public async Task<Result> DeleteInstructor(InstructorKey id)
    {
        var deleted = await _instructorRepository.Delete(id);
        if (!deleted)
            return new InstructorNotFoundException();
        await _instructorRepository.Save();
        return Result.Success();
    }
}