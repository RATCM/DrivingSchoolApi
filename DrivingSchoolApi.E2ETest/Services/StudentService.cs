using System.Net.Http.Headers;
using System.Net.Http.Json;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.Student;

namespace DrivingSchoolApi.E2ETest.Services;

public class StudentService
{
    private readonly HttpClient _client;
    private readonly AuthService _authService;
    private JwtTokenDto? Bearer => _authService.Bearer;

    public StudentService(HttpClient client, AuthService authService)
    {
        _client = client;
        _authService = authService;
    }

    public async Task<HttpResponseMessage> GetAllStudents(int page = 1)
    {
        using var getStudentsRequest =
            new HttpRequestMessage(HttpMethod.Get, $"student?page={page}");
        getStudentsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        
        return await _client.SendAsync(getStudentsRequest);
    }
    
    public async Task<HttpResponseMessage> GetTheoryLessons(int page = 1)
    {
        throw new NotImplementedException("Not implemented");
    }

    public async Task<HttpResponseMessage> CreateStudent(StudentRegistryDto registry)
    {
        return await _client.PostAsJsonAsync("student", registry);
    }
    
    public async Task<HttpResponseMessage> GetStudentById(Guid id)
    {
        using var getStudentRequest =
            new HttpRequestMessage(HttpMethod.Get, $"student/{id}");
        getStudentRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);

        return await _client.SendAsync(getStudentRequest);
    }
    
}