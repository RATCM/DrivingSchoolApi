using DrivingSchoolApi.Application.UnitTest.Extensions;
using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class CompletedCourseServiceTests
{
    [Test]
    public async Task CreateCompletedCourse_ReturnsCompletedCourse_AndSaves_OnSuccess()
    {
        var completedCourse = CompletedCourse.CreateTestCompletedCourse();
        
    }
    
    
}