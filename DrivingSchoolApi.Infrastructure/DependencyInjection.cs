using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            return services
                .AddScoped<IDrivingLessonRepository, DrivingLessonRepository>()
                .AddScoped<IDrivingSchoolRepository, DrivingSchoolRepository>()
                .AddScoped<IInstructorRepository, InstructorRepository>()
                .AddScoped<IStudentRepository, StudentRepository>()
                .AddScoped<ITheoryLessonRepository, TheoryLessonRepository>()
                .AddDbContext<IDrivingSchoolDbContext, DrivingSchoolDbContext>();
        }
    }
}