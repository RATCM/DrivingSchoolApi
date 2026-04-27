using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class CompletedCourseServiceTests
{
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();
        collection
            .AddScoped<ICompletedCourseService, CompletedCourseService>()
            .AddScoped<ICompletedCourseRepository>(_ => Substitute.For<ICompletedCourseRepository>())
            .AddScoped<IDateTimeProviderService>(_  => Substitute.For<IDateTimeProviderService>())
            .AddScoped<IGuidGeneratorService>(_ =>  Substitute.For<IGuidGeneratorService>())
            .AddScoped<IStudentService>(_  => Substitute.For<IStudentService>())
            .AddScoped<ITheoryLessonService>(_ => Substitute.For<ITheoryLessonService>())
            .AddScoped<IDrivingLessonService>(_ => Substitute.For<IDrivingLessonService>())
            .AddScoped<IDrivingSchoolService>(_ => Substitute.For<IDrivingSchoolService>());

        _serviceProvider = collection.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public async Task CreateCompletedCourse_ReturnsCompletedCourse_AndSaves_OnSuccess()
    {
        var completedCourse = CompletedCourse.CreateTestCompletedCourse();
        
    }
    
    
}