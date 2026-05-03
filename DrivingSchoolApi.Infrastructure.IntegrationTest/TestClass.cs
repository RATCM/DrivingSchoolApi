using System.Data.Common;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrivingSchoolApi.Infrastructure.IntegrationTest;

public class TestClass
{
    protected ServiceProvider ServiceProvider { get; private set; }
    
    protected IStudentRepository GetStudentRepository()
    {
        return ServiceProvider.GetRequiredService<IStudentRepository>();
    }
    
    protected IInstructorRepository GetInstructorRepository()
    {
        return ServiceProvider.GetRequiredService<IInstructorRepository>();
    }
    
    protected IDrivingSchoolRepository GetDrivingSchoolRepository()
    {
        return ServiceProvider.GetRequiredService<IDrivingSchoolRepository>();
    }

    protected IStudentInviteRepository GetStudentInviteRepository()
    {
        return ServiceProvider.GetRequiredService<IStudentInviteRepository>();
    }

    
    [SetUp]
    public async Task Setup()
    {
        var collection = new ServiceCollection();

        collection
            .AddSingleton<DbConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

        collection.AddDbContext<IDrivingSchoolDbContext, TestDbContext>((sp, options) =>
        {
            var conn = sp.GetRequiredService<DbConnection>();
            options.UseSqlite(conn);
        });

        collection
            .AddScoped<IAdminRepository, AdminRepository>()
            .AddScoped<ICompletedCourseRepository, CompletedCourseRepository>()
            .AddScoped<IDrivingLessonRepository, DrivingLessonRepository>()
            .AddScoped<IDrivingSchoolRepository, DrivingSchoolRepository>()
            .AddScoped<IInstructorRepository, InstructorRepository>()
            .AddScoped<IStudentInviteRepository, StudentInviteRepository>()
            .AddScoped<IStudentRepository, StudentRepository>()
            .AddScoped<ITheoryLessonRepository, TheoryLessonRepository>();

        ServiceProvider = collection.BuildServiceProvider();
        
        var db = ServiceProvider.GetRequiredService<TestDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        var db = ServiceProvider.GetRequiredService<TestDbContext>();
        await db.Database.EnsureDeletedAsync();
        await ServiceProvider.DisposeAsync();
    }

}