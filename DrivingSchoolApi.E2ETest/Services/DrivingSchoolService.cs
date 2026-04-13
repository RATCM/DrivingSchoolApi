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

    public async Task<HttpResponseMessage> GetAllDrivingSchools()
    {
        throw new NotImplementedException("Not implemented");
    }
    

    public async Task<HttpResponseMessage> CreateDrivingSchool(DrivingSchoolRegistryDto registry)
    {
        using var createSchoolRequest = new HttpRequestMessage(HttpMethod.Post, "/drivingSchool");
        createSchoolRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        createSchoolRequest.Content = JsonContent.Create(registry);
        
        return await _client.SendAsync(createSchoolRequest);
    }
    
    public async Task<HttpResponseMessage> GetAllStudentsFromSchool()
    {
        throw new NotImplementedException("Not implemented");
    }
    
    public async Task<HttpResponseMessage> CreateInvite(Guid schoolId)
    {
        using var createInviteRequest =
            new HttpRequestMessage(HttpMethod.Post, $"/drivingSchool/{schoolId}/student/invite");
        createInviteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);

        return await _client.SendAsync(createInviteRequest);
    }
}