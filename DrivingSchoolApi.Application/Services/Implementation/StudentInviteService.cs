using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Exceptions.StudentInvite;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Application.Services.Implementation;

public class StudentInviteService : IStudentInviteService
{
    private readonly IStudentInviteRepository _studentInviteRepository;
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IDateTimeProviderService _dateTimeProviderService;

    public StudentInviteService(
        IStudentInviteRepository studentInviteRepository,
        IDrivingSchoolRepository drivingSchoolRepository,
        IDateTimeProviderService dateTimeProviderService)
    {
        _studentInviteRepository = studentInviteRepository;
        _drivingSchoolRepository = drivingSchoolRepository;
        _dateTimeProviderService = dateTimeProviderService;
    }
    
    public async Task<Result<DrivingSchool>> RedeemStudentInvite(StudentInviteKey id)
    {
        var invite = await _studentInviteRepository.Get(id);
        if (invite is null)
            return new StudentInviteNotFoundException("Student invite not found");
        
        // If this happens, then there must be some issue with the database
        // connection, for now we just throw an exception
        var deleted = await _studentInviteRepository.Delete(id);
        if (!deleted) 
            throw new Exception("Some error happened");

        // We check the expiration datetime
        if (invite.ExpirationDateTime < _dateTimeProviderService.Now())
            return new StudentInviteExpiredException("Student invite has expired");
        
        var drivingSchool = await _drivingSchoolRepository.Get(invite.DrivingSchoolId);
        if (drivingSchool is null)
            return new DrivingSchoolNotFoundException("Driving school not found");

        await _studentInviteRepository.Save();
        
        return drivingSchool;
    }
}