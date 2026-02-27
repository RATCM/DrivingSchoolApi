using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services.Implementation;

internal class InstructorService : IInstructorService
{
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IInstructorRepository _instructorRepository;

    public InstructorService(
        IGuidGeneratorService guidGeneratorService,
        IInstructorRepository instructorRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _instructorRepository = instructorRepository;
    }
    
    public async Task<Instructor> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task<Instructor> GetInstructorById(InstructorKey id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Instructor>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteInstructor(InstructorKey id)
    {
        throw new NotImplementedException();
    }
}