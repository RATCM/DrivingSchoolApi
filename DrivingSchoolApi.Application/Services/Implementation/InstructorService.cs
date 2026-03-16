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

    public InstructorService(
        IGuidGeneratorService guidGeneratorService,
        IInstructorRepository instructorRepository)
    {
        _guidGeneratorService = guidGeneratorService;
        _instructorRepository = instructorRepository;
    }
    
    public async Task<Result<Instructor>> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, DrivingSchoolKey schoolId)
    {
        return new NotImplementedException();
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

    public async Task<Result> DeleteInstructor(InstructorKey id)
    {
        var deleted = await _instructorRepository.Delete(id);
        if (!deleted)
            return new InstructorNotFoundException();
        await _instructorRepository.Save();
        return Result.Success();
    }
}