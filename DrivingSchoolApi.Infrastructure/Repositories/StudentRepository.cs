using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class StudentRepository : Repository, IStudentRepository
{
    public StudentRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(Student student)
    {
        var entry = await DbContext.Students.AddAsync(student);

        return entry.State == EntityState.Added;
    }

    public async Task<Student?> Get(StudentKey id)
    {
        return await DbContext.Students.FindAsync(id);
    }
    
    public async Task<IEnumerable<Student>> GetAll()
    {
        return DbContext.Students;
    }

    public async Task<bool> Update(Student student)
    {
        var temp = await DbContext.Students.FindAsync(student.Id);
        if (temp is null) return false;
        var entry = DbContext.Students.Update(student);
        return entry.State is EntityState.Modified or EntityState.Unchanged;
    }

    public async Task<bool> Delete(StudentKey id)
    {
        var temp = await DbContext.Students.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.Students.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}