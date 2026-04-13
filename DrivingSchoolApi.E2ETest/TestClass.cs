using DrivingSchoolApi.Application.Services;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.DTOs.Admin;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.E2ETest.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrivingSchoolApi.E2ETest;

public abstract class TestClass
{
    private HttpClient _client;
    private TestApplicationFactory _factory;
    protected Admin AdminUser { get; private set; }
    protected AuthService AuthService;
    protected DrivingSchoolService DrivingSchoolService;

    [SetUp]
    public async Task SetUp()
    {
        _factory = new TestApplicationFactory();
        _client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TestDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        var adminService = services.GetRequiredService<IAdminService>();
        
        var adminResult = await adminService.CreateAdmin(
            Email.Create("admin@test"),
            "AdminPassword1!");

        AdminUser = adminResult.Value!;
        
        AuthService = new AuthService(_client);
        DrivingSchoolService = new DrivingSchoolService(_client, AuthService);
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await _factory.DisposeAsync();
        _client.Dispose();
    }
}