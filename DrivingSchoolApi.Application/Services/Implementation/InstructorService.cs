using DrivingSchoolApi.Application.Enums;
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
        var instructor = await _instructorRepository.GetByEmail(Email.Create(email));
        if (instructor is null)
            return new InstructorNotFoundException("Instructor not found during login attempt.");

        if (!_passwordHasherService.VerifyHashedPassword(password, instructor.HashedPassword))
            return new InvalidLoginRequestException();
        
        var accessToken = _tokenGeneratorService.GenerateJwtAccessToken(instructor.Id.Value, UserRole.Instructor);
        var refreshToken = _tokenGeneratorService.GenerateJwtRefreshToken(instructor.Id.Value, UserRole.Instructor);
        return (accessToken, refreshToken);
    }
    
    public async Task<Result<Instructor>> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        var instructor = Instructor.Create(
            InstructorKey.Create(_guidGeneratorService.NewGuid()),
            schoolId,
            name,
            email,
            _passwordHasherService.HashPassword(password),
            phoneNumber);
        
        var result = await _instructorRepository.Create(instructor);

        if(!result)
            return new Exception("Unable to create instructor.");
        
        await _instructorRepository.Save();
        return instructor;
    }

    //Admin only
    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructors()
    {
        var instructors = await _instructorRepository.GetAll();
        return instructors.ToList();
    }

    public async Task<Result<Instructor>> GetInstructorById(InstructorKey id)
    {
        var result = await _instructorRepository.Get(id);
        return result is not null
            ? result 
            : new InstructorNotFoundException("Instructor not found in DB.");
    }

    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId)
    {
        var instructors = await _instructorRepository.GetAll();
        
        return instructors.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    // Used for SameDrivingSchoolFilterService
    public async Task<Result<DrivingSchoolKey>> GetInstructorDrivingSchoolId(InstructorKey id)
    {
        var instructor = await _instructorRepository.Get(id);
        return instructor is null
            ? new InstructorNotFoundException("Instructor not found in DB.")
            : instructor.SchoolId;
    }

    public async Task<Result<Instructor>> UpdateInstructor(InstructorKey id, Name name, Email email, PhoneNumber phoneNumber)
    {
        var instructorResult = await _instructorRepository.Get(id);
        if (instructorResult is null)
            return new InstructorNotFoundException("Instructor not found in DB.");

        var updatedInstructor = Instructor.Create(
            instructorResult.Id,
            instructorResult.SchoolId,
            name,
            email, 
            instructorResult.HashedPassword,
            phoneNumber);
        
        bool succes = await _instructorRepository.Update(updatedInstructor);

        if (!succes)
            return  new Exception("Internal error: Unable to update instructor.");
        
        await _instructorRepository.Save();
        return updatedInstructor;
    }

    public async Task<Result> UpdateInstructorPassword(InstructorKey id, string oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
            return new InvalidPasswordException("New password cannot be the same as the old password.");
        
        var instructorResult = await _instructorRepository.Get(id);
        if (instructorResult is null)
            return new InstructorNotFoundException("Instructor not found in DB.");
        
        var correctPassword = _passwordHasherService.VerifyHashedPassword(oldPassword, instructorResult.HashedPassword);

        if (!correctPassword)
            return new InvalidPasswordException("Old password doesn't match.");
        
        var updatedInstructor = Instructor.Create(
            instructorResult.Id,
            instructorResult.SchoolId,
            instructorResult.InstructorName,
            instructorResult.EmailAddress,
            _passwordHasherService.HashPassword(newPassword),
            instructorResult.PhoneNumber);
        
        bool succes = await _instructorRepository.Update(updatedInstructor);

        if (!succes)
            return  new Exception("Internal error: Unable to update instructor.");
        
        await _instructorRepository.Save();
        return Result.Success();
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
