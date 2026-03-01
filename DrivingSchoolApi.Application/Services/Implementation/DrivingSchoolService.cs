using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
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
    
    public async Task<DrivingSchool> CreateDrivingSchool(DrivingSchoolName name, Address address, PhoneNumber phoneNumber, WebAddress webAddress)
    {
        var drivingSchool = DrivingSchool.Create(
             DrivingSchoolKey.Create(_guidGeneratorService.NewGuid()),
            name,
            address,
            phoneNumber,
            webAddress);
        
        var result = await _drivingSchoolRepository.Create(drivingSchool);

        if(!result)
            throw new Exception("Unable to create driving school");
        
        await _drivingSchoolRepository.Save();
        return drivingSchool;
    }

    public async Task<DrivingSchool> GetDrivingSchoolById(DrivingSchoolKey id)
    {
        return await _drivingSchoolRepository.Get(id) ?? throw new DrivingSchoolNotFoundException();
    }
    
    public async Task<IEnumerable<DrivingSchool>> GetAllDrivingSchools()
    {
        return await _drivingSchoolRepository.GetAll();
    }

    public async Task DeleteDrivingSchool(DrivingSchoolKey id)
    {
        var deleted = await _drivingSchoolRepository.Delete(id);
        if (!deleted)
            throw new DrivingSchoolNotFoundException();
        await _drivingSchoolRepository.Save();
    }
}