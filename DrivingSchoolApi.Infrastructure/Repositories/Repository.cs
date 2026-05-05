using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Infrastructure.Database;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal abstract class Repository : IRepository
{
    protected readonly IDrivingSchoolDbContext DbContext;

    public Repository(IDrivingSchoolDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<Result> Save()
    {
        try
        {
            await DbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
