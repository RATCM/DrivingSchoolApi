using DrivingSchoolApi.Application.Exceptions.DrivingSchool;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public class DrivingSchoolService : IDrivingSchoolService
{
    private readonly IDrivingSchoolRepository _drivingSchoolRepository;
    public DrivingSchoolService(IDrivingSchoolRepository drivingSchoolRepository)
    {
        _drivingSchoolRepository = drivingSchoolRepository;
    }
    
    public async Task<DrivingSchool> CreateDrivingSchool(Address address, PhoneNumber phoneNumber, WebAddress webAddress)
    {
        throw new NotImplementedException();
    }

    public async Task<DrivingSchool> GetDrivingSchoolById(Guid id)
    {
        return await _drivingSchoolRepository.Get(id) ?? throw new DrivingSchoolNotFoundException();
    }

    public async Task<IEnumerable<DrivingSchool>> GetAllDrivingSchools()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDrivingSchool(Guid id)
    {
        throw new NotImplementedException();
    }
}