using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            return services
                .AddScoped<IGuidGeneratorService, GuidGeneratorService>()
                .AddScoped<IDrivingLessonService, DrivingLessonService>()
                .AddScoped<IDrivingSchoolService, DrivingSchoolService>()
                .AddScoped<IInstructorService, InstructorService>()
                .AddScoped<IStudentService, StudentService>()
                .AddScoped<ITheoryLessonService, TheoryLessonService>()
                .AddScoped<IStudentInviteService, StudentInviteService>()
                .AddScoped<IDateTimeProviderService, DateTimeProviderService>();
        }
    }
}