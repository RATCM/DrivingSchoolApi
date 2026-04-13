using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using DrivingSchoolApi.DTOs.Common;
using DrivingSchoolApi.DTOs.DrivingLesson;
using DrivingSchoolApi.DTOs.Instructor;
using DrivingSchoolApi.DTOs.TheoryLesson;

namespace DrivingSchoolApi.E2ETest.Services;

public class InstructorService
{
    private readonly HttpClient _client;
    private readonly AuthService _authService;
    private JwtTokenDto? Bearer => _authService.Bearer;

    public InstructorService(HttpClient client, AuthService authService)
    {
        _client = client;
        _authService = authService;
    }

    public async Task<HttpResponseMessage> CreateInstructor(InstructorRegistryDto registry)
    {
        using var createInstructorRequest =
            new HttpRequestMessage(HttpMethod.Post, "instructor/register");
        createInstructorRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        createInstructorRequest.Content = JsonContent.Create(registry);

        return await _client.SendAsync(createInstructorRequest);
    }

    public async Task<HttpResponseMessage> GetAllInstructors()
    {
        using var getInstructorsRequest =
            new HttpRequestMessage(HttpMethod.Get, "instructor");
        getInstructorsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);

        return await _client.SendAsync(getInstructorsRequest);
    }
    
    public async Task<HttpResponseMessage> GetInstructorById(Guid id)
    {
        using var getInstructorRequest =
            new HttpRequestMessage(HttpMethod.Get, $"instructor/{id}");
        getInstructorRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);

        return await _client.SendAsync(getInstructorRequest);
    }
    
    public async Task<HttpResponseMessage> UpdateInstructor(Guid id, InstructorUpdateDto update)
    {
        using var updateInstructorRequest =
            new HttpRequestMessage(HttpMethod.Put, $"instructor/{id}");
        updateInstructorRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        updateInstructorRequest.Content = JsonContent.Create(update);

        return await _client.SendAsync(updateInstructorRequest);
    }

    public async Task<HttpResponseMessage> UpdateInstructorPassword(Guid id, UpdatePasswordDto update)
    {
        using var updatePasswordRequest =
            new HttpRequestMessage(HttpMethod.Put, $"instructor/{id}/password");
        updatePasswordRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        updatePasswordRequest.Content = JsonContent.Create(update);

        return await _client.SendAsync(updatePasswordRequest);
    }

    public async Task<HttpResponseMessage> DeleteInstructor(Guid id)
    {
        using var deletePasswordRequest =
            new HttpRequestMessage(HttpMethod.Delete, $"instructor/{id}");
        deletePasswordRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        
        return await _client.SendAsync(deletePasswordRequest);
    }

    public async Task<HttpResponseMessage> CreateTheoryLesson(Guid id, TheoryLessonRegistryDto registry)
    {
        using var createTheoryLessonRequest =
            new HttpRequestMessage(HttpMethod.Post, $"instructor/{id}/theoryLesson");
        createTheoryLessonRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        createTheoryLessonRequest.Content = JsonContent.Create(registry);

        return await _client.SendAsync(createTheoryLessonRequest);
    }

    public async Task<HttpResponseMessage> GetTheoryLessons(Guid id)
    {
        using var getTheoryLessonRequest =
            new HttpRequestMessage(HttpMethod.Get, $"instructor/{id}/theoryLesson");
        getTheoryLessonRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        
        return await _client.SendAsync(getTheoryLessonRequest);
    }
    
    public async Task<HttpResponseMessage> CreateDrivingLesson(Guid id, DrivingLessonRegistryDto registry)
    {
        using var createDrivingLessonRequest =
            new HttpRequestMessage(HttpMethod.Post, $"instructor/{id}/drivingLesson");
        createDrivingLessonRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        createDrivingLessonRequest.Content = JsonContent.Create(registry);

        return await _client.SendAsync(createDrivingLessonRequest);
    }
    
    public async Task<HttpResponseMessage> GetDrivingLessons(Guid id)
    {
        using var getDrivingLessonRequest =
            new HttpRequestMessage(HttpMethod.Get, $"instructor/{id}/drivingLesson");
        getDrivingLessonRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Bearer?.AccessToken);
        
        return await _client.SendAsync(getDrivingLessonRequest);
    }
}