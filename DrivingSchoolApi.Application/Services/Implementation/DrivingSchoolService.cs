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
    public DrivingSchoolService(
        IGuidGeneratorService guidGeneratorService,
        IDrivingSchoolRepository drivingSchoolRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _drivingSchoolRepository = drivingSchoolRepository;
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
            return new DrivingSchoolNotFoundException();
        return drivingSchool;
    }
    
    public async Task<Result<IEnumerable<DrivingSchool>>> GetAllDrivingSchools()
    {
        return (await _drivingSchoolRepository.GetAll()).ToList();
    }

    public async Task<Result> DeleteDrivingSchool(DrivingSchoolKey id)
    {
        var deleted = await _drivingSchoolRepository.Delete(id);
        if (!deleted)
            return new DrivingSchoolNotFoundException();
        await _drivingSchoolRepository.Save();
        return Result.Success();
    }
}