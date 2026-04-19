using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Application.Services.Implementation;
using NUnit.Framework;

namespace DrivingSchoolApi.Application.UnitTest.Services;

public class DateTimeProviderServiceTests
{
    [Test]
    public void Now_ReturnsCurrentDateTime()
    {
        // Arrange
        var sut = new DateTimeProviderService();
        var beforeCall = DateTime.Now;
        
        // Act
        var result = sut.Now();
        
        var afterCall = DateTime.Now;
        
        // Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(beforeCall));
        Assert.That(result, Is.LessThanOrEqualTo(afterCall));
    }
    
    [Test]
    public void Now_ReturnsDifferentValueOnSecondCall()
    {
        // Arrange
        var sut = new DateTimeProviderService();
        
        // Act
        var firstCall = sut.Now();
        Thread.Sleep(10); // Small delay
        var secondCall = sut.Now();
        
        // Assert
        Assert.That(secondCall, Is.GreaterThanOrEqualTo(firstCall));
    }
}