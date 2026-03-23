using DrivingSchoolApi.Application.Exceptions.Admin;
using DrivingSchoolApi.Application.Exceptions.Common;
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
    private readonly IAdminRepository _adminRepository;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IPasswordHasher<Instructor> _passwordHasherService;

    public InstructorService(
        IGuidGeneratorService guidGeneratorService,
        IInstructorRepository instructorRepository,
        IAdminRepository adminRepository,
        ITokenGeneratorService tokenGeneratorService,
        IPasswordHasher<Instructor> passwordHasher)
    {
        _guidGeneratorService = guidGeneratorService;
        _instructorRepository = instructorRepository;
        _adminRepository = adminRepository;
        _tokenGeneratorService = tokenGeneratorService;
        _passwordHasherService = passwordHasher;
    }

    public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsInstructor(string email, string password)
    {
        var instructor = await _instructorRepository.GetByEmail(Email.Create(email));
        if (instructor is null)
            return new InstructorNotFoundException("Instructor not found during login attempt.");

        if (!_passwordHasherService.VerifyHashedPassword(password, instructor.HashedPassword))
            return new InvalidLoginRequestException();
        
        var accessToken = _tokenGeneratorService.GenerateJwtAccessToken(instructor.Id.Value, "Instructor");
        var refreshToken = _tokenGeneratorService.GenerateJwtRefreshToken(instructor.Id.Value, "Instructor");
        return (accessToken, refreshToken);
    }
    
    public async Task<Result<Instructor>> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        return new NotImplementedException();
    }

    //Admin only
    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructors()
    {
        var instructors = await _instructorRepository.GetAll();
        return instructors.ToList();
    }

    public async Task<Result<Instructor>> GetInstructorById(Guid claimedId, bool isAdmin, InstructorKey requestedId)
    {
        // Probably overkill to check if admin exists in DB
        if (isAdmin)
        {
            var resultAdmin = await _adminRepository.Get(AdminKey.Create(claimedId));
            if (resultAdmin is null)
                return new AdminNotFoundException("Couldn't find your admin account in DB.");
        }
        
        var searchId = isAdmin ? requestedId : InstructorKey.Create(claimedId);
        
        var result = await _instructorRepository.Get(searchId);
        
        return result is not null ? 
            result : 
            new InstructorNotFoundException("Instructor not found in DB.");
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
            return new InstructorNotFoundException("Instructor not found in DB.");
        await _instructorRepository.Save();
        return Result.Success();
    }
}