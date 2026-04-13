using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;

namespace DrivingSchoolApi.E2ETest.Services;

public class AuthService
{
    private readonly HttpClient _client;
    public JwtTokenDto? Bearer { get; private set; }

    public AuthService(HttpClient client)
    {
        _client = client;
    }

    public async Task<JwtTokenDto> LoginAsDefaultAdmin()
    {
        var tokenResponse = await _client.PostAsJsonAsync(
            "/admin/login",
            new LoginDto("admin@test", "AdminPassword1!"));
        
        Bearer = await tokenResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        return Bearer ?? 
               throw new Exception("Unable to login as admin"); // Should not happen
    }

    public async Task<JwtTokenDto?> LoginAdmin(LoginDto dto)
    {
        var tokenResponse = await _client.PostAsJsonAsync(
            "/admin/login",
            dto);
        
        Bearer = await tokenResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        return Bearer;
    }
    
    public async Task<JwtTokenDto?> LoginStudent(LoginDto dto)
    {
        var tokenResponse = await _client.PostAsJsonAsync(
            "/student/login",
            dto);
        
        Bearer = await tokenResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        return Bearer;
    }
    
    public async Task<JwtTokenDto?> LoginInstructor(LoginDto dto)
    {
        var tokenResponse = await _client.PostAsJsonAsync(
            "/instructor/login",
            dto);
        
        Bearer = await tokenResponse.Content.ReadFromJsonAsync<JwtTokenDto>();
        return Bearer;
    }
}