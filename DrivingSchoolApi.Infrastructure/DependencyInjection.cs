using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Identity;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            services
                .AddScoped<IDrivingLessonRepository, DrivingLessonRepository>()
                .AddScoped<IDrivingSchoolRepository, DrivingSchoolRepository>()
                .AddScoped<IInstructorRepository, InstructorRepository>()
                .AddScoped<IStudentRepository, StudentRepository>()
                .AddScoped<ITheoryLessonRepository, TheoryLessonRepository>()
                .AddScoped<ITokenGeneratorService, TokenGeneratorService>()
                .AddDbContext<IDrivingSchoolDbContext, DrivingSchoolDbContext>();

            services
                .AddAuthentication(AuthSchemes.Access)
                .AddJwtBearer(AuthSchemes.Access)
                .AddJwtBearer(AuthSchemes.Refresh);

            services.AddAuthorizationBuilder()
                .AddPolicy(AuthPolicies.AdminOnly, policy => policy.RequireRole("Admin"));

            return services;
        }
    }
}