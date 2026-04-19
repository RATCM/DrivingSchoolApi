using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class DrivingSchoolService : IDrivingSchoolService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    private readonly IDateTimeProviderService _dateTimeProviderService;
    public DrivingSchoolService(
        IGuidGeneratorService guidGeneratorService,
        IDrivingSchoolRepository drivingSchoolRepository,
        IDateTimeProviderService dateTimeProviderService)
    {
        _guidGeneratorService = guidGeneratorService;
        _drivingSchoolRepository = drivingSchoolRepository;
        _dateTimeProviderService = dateTimeProviderService;
    }
    
    public async Task<Result<DrivingSchool>> CreateDrivingSchool(DrivingSchoolName name, StreetAddress address, PhoneNumber phoneNumber, WebAddress webAddress, Package[] packages)
    {
        var drivingSchool = DrivingSchool.Create(
             DrivingSchoolKey.Create(_guidGeneratorService.NewGuid()),
            name,
            address,
            phoneNumber,
            webAddress,
            packages);
        
        var result = await _drivingSchoolRepository.Create(drivingSchool);

        if(!result)
            return new Exception("Unable to create driving school");
        
        await _drivingSchoolRepository.Save();
        return drivingSchool;
    }

    public async Task<Result<DrivingSchool>> GetDrivingSchoolById(DrivingSchoolKey id)
    {
        var drivingSchool = await _drivingSchoolRepository.Get(id);
        if(drivingSchool is null)
            return new DrivingSchoolNotFoundException("Driving school not found.");
        return drivingSchool;
    }
    
    public async Task<Result<IEnumerable<DrivingSchool>>> GetAllDrivingSchools()
    {
        var drivingSchools = await _drivingSchoolRepository.GetAll();
        return drivingSchools.ToList();
    }

    public async Task<Result<StudentInvite>> CreateStudentInvite(DrivingSchoolKey id, TimeSpan timeValid)
    {
        var drivingSchool = await _drivingSchoolRepository.Get(id);
        if (drivingSchool is null)
            return new DrivingSchoolNotFoundException("Driving school not found");

        var invite = StudentInvite.Create(
            StudentInviteKey.Create(_guidGeneratorService.NewGuid()),
            id,
            DateTime.Now.Add(timeValid));
        
        drivingSchool.AddStudentInvite(invite);

        return invite;
    }

    public async Task<Result> DeleteDrivingSchool(DrivingSchoolKey id)
    {
        var deleted = await _drivingSchoolRepository.Delete(id);
        if (!deleted)
            return new DrivingSchoolNotFoundException("Driving school not found.");
        await _drivingSchoolRepository.Save();
        return Result.Success();
    }

    public async Task<Result<DrivingSchool>> UpdateDrivingSchool(DrivingSchoolKey id, DrivingSchoolName name,
        StreetAddress streetAddress, PhoneNumber phoneNumber, WebAddress webAddress)
    {
        var result = await _drivingSchoolRepository.Get(id);
        if (result is null)
        {
            return new DrivingSchoolNotFoundException("Driving school not found in DB");
        }

        var updatedDrivingSchool = DrivingSchool.Create(
            result.Id,
            name,
            streetAddress,
            phoneNumber,
            webAddress,
            result.Packages.ToArray());

        bool success = await _drivingSchoolRepository.Update(updatedDrivingSchool);

        if (!success)
        {
            return new Exception("Internal error: Unable to update driving school.");
        }

        await _drivingSchoolRepository.Save();
        return updatedDrivingSchool;
    }
}
