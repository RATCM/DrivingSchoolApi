using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface IStudentRepository
{
    Task<bool> Create(Student student);
    Task<Student?> Get(StudentKey id);
    Task<IEnumerable<Student>> GetAll();
    Task<bool> Update(Student student);
    Task<bool> Delete(StudentKey id);
    Task Save();
}