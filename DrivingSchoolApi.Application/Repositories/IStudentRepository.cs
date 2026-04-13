using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Repositories;

public interface IStudentRepository
{
    Task<bool> Create(Student student);
    Task<Student?> Get(StudentKey id);
    Task<Student?> GetByEmail(Email email);
    Task<IEnumerable<Student>> GetAll();
    Task<IEnumerable<Student>> GetAllFromDrivingSchool(DrivingSchoolKey id);
    Task<bool> Update(Student student);
    Task<bool> Delete(StudentKey id);
    Task Save();
}