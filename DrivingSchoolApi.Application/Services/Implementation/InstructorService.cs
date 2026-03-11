using DrivingSchoolApi.Application.Exceptions.Instructor;
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
        return await _instructorRepository.Get(id) ?? throw new InstructorNotFoundException();
    }

    public async Task<IEnumerable<Instructor>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId)
    {
        var instructors = await _instructorRepository.GetAll();
        
        return instructors.Where(x => x.SchoolId.Equals(schoolId)).ToList();
    }

    public async Task DeleteInstructor(InstructorKey id)
    {
        var deleted = await _instructorRepository.Delete(id);
        if (!deleted)
            throw new InstructorNotFoundException();
        await _instructorRepository.Save();
    }
}