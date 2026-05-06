using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class InstructorRepository : Repository, IInstructorRepository
{
    public InstructorRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(Instructor instructor)
    {
        var entry = await DbContext.Instructors.AddAsync(instructor);

        return entry.State == EntityState.Added;
    }

    public async Task<Instructor?> Get(InstructorKey id)
    {
        return await DbContext.Instructors.FindAsync(id);
    }

    public async Task<Instructor?> GetByEmail(Email email)
    {
        return await DbContext.Instructors
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.EmailAddress == email);
    }
    
    public async Task<IEnumerable<Instructor>> GetAll()
    {
        return await DbContext.Instructors.AsNoTracking().ToListAsync();
    }

    public async Task<bool> Update(Instructor instructor)
    {
        var temp = await DbContext.Instructors.FindAsync(instructor.Id);
        if (temp is null) return false;
        var timeSlots = temp.Calender.TimeSlots.ToList();
        foreach (var timeSlot in timeSlots)
            temp.Calender.RemoveTimeSlot(timeSlot);
        
        if(temp.EmailAddress != instructor.EmailAddress) temp.ChangeEmail(instructor.EmailAddress);
        if(temp.InstructorName != instructor.InstructorName) temp.ChangeName(instructor.InstructorName);
        if(!temp.SchoolId.Equals(instructor.SchoolId)) temp.ChangeSchool(instructor.SchoolId);
        if(temp.HashedPassword != instructor.HashedPassword) temp.ChangePasswordHash(instructor.HashedPassword);
        if(temp.PhoneNumber != instructor.PhoneNumber) temp.ChangePhoneNumber(instructor.PhoneNumber);
        
        foreach(var timeSlot in instructor.Calender.TimeSlots)
            temp.Calender.AddTimeSlot(timeSlot);
        
        var entry = DbContext.Instructors.Update(temp);
        return entry.State is EntityState.Modified or EntityState.Unchanged;
    }

    public async Task<bool> Delete(InstructorKey id)
    {
        var temp = await DbContext.Instructors.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.Instructors.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}
