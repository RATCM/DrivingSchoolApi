using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.Repositories;

public interface IStudentRepository
{
    Task<bool> Create(Student student);
    Task<Student?> Get(Guid id);
    Task<IEnumerable<Student>> GetAll();
    Task<bool> Update(Student student);
    Task<bool> Delete(Guid id);
}