using DrivingSchoolApi.Application.Auth;
using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Infrastructure.Database;
using DrivingSchoolApi.Infrastructure.Identity;
using DrivingSchoolApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
                .AddScoped<IPasswordHasher<Student>, PasswordHasher<Student>>()
                .AddScoped<IPasswordHasher<Instructor>, PasswordHasher<Instructor>>()
                .AddScoped<IPasswordHasher<Admin>, PasswordHasher<Admin>>()
                .AddDbContext<IDrivingSchoolDbContext, DrivingSchoolDbContext>();

            services
                .AddAuthentication(AuthSchemes.Access)
                .AddJwtBearer(AuthSchemes.Access)
                .AddJwtBearer(AuthSchemes.Refresh);

            services.AddAuthorizationBuilder()
                .AddPolicy(AuthPolicies.AdminOnly, policy => policy.RequireRole("Admin"))
                .AddPolicy(AuthPolicies.InstructorOnly, policy => policy.RequireRole("Instructor"))
                .AddPolicy(AuthPolicies.StudentOnly, policy => policy.RequireRole("Student"));

            return services;
        }
    }

    extension(WebApplication app)
    {
        public void ApplyMigrations()
        {
            using var scope = app.Services.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<DrivingSchoolDbContext>();
            
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
                dbContext.Database.Migrate();
        }
    }
}