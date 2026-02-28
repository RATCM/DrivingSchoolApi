using DrivingSchoolApi.Infrastructure.Database;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal abstract class Repository
{
    protected readonly IDrivingSchoolDbContext DbContext;

    public Repository(IDrivingSchoolDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task Save()
    {
        await DbContext.SaveChangesAsync();
    }
}