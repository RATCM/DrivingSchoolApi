using System.Net.Http.Headers;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingSchool;

namespace DrivingSchoolApi.E2ETest.Services;

public class DrivingSchoolService
{
    private readonly HttpClient _client;
    private readonly AuthService _authService;
    private JwtTokenDto? Bearer => _authService.Bearer;

    public DrivingSchoolService(HttpClient client, AuthService authService)
    {
        _client = client;
        _authService = authService;
    }

    public async Task<HttpResponseMessage> GetDrivingSchool(Guid schoolId)
    {
        using var getSchoolRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}");
        return await _client.SendAsync(getSchoolRequest);
    }

    public async Task<HttpResponseMessage> GetAllDrivingSchools()
    {
        using var getAllSchoolRequests = new HttpRequestMessage(HttpMethod.Get, "drivingSchool");

        return await _client.SendAsync(getAllSchoolRequests);
    }
    

    public async Task<HttpResponseMessage> CreateDrivingSchool(DrivingSchoolRegistryDto registry)
    {
        using var createSchoolRequest = new HttpRequestMessage(HttpMethod.Post, "/drivingSchool");
        createSchoolRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        createSchoolRequest.Content = JsonContent.Create(registry);
        
        return await _client.SendAsync(createSchoolRequest);
    }
    
    public async Task<HttpResponseMessage> GetAllStudentsFromSchool(Guid schoolId)
    {
        using var getStudentsRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}/student");
        getStudentsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        return await _client.SendAsync(getStudentsRequest);
    }
    
    public async Task<HttpResponseMessage> CreateInvite(Guid schoolId)
    {
        using var createInviteRequest =
            new HttpRequestMessage(HttpMethod.Post, $"/drivingSchool/{schoolId}/student/invite");
        createInviteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);

        return await _client.SendAsync(createInviteRequest);
    }

    public async Task<HttpResponseMessage> GetDrivingSchoolRating(Guid schoolId)
    {
        using var getRatingRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}/rating");
        return await _client.SendAsync(getRatingRequest);
    }

    public async Task<HttpResponseMessage> GetAllInstructorsFromSchool(Guid schoolId)
    {
        using var getInstructorsRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}/Instructor");
        getInstructorsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        return await _client.SendAsync(getInstructorsRequest);
    }
    
    public async Task<HttpResponseMessage> GetDrivingSchoolTheoryLessons(Guid schoolId)
    {
        using var getTheoryLessonsRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}/theoryLesson");
        getTheoryLessonsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        return await _client.SendAsync(getTheoryLessonsRequest);
    }
    
    public async Task<HttpResponseMessage> GetDrivingSchoolDrivingLessons(Guid schoolId)
    {
        using var getDrivingLessonsRequest = new HttpRequestMessage(HttpMethod.Get, $"/drivingSchool/{schoolId}/drivingLesson");
        getDrivingLessonsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        return await _client.SendAsync(getDrivingLessonsRequest);
    }
}