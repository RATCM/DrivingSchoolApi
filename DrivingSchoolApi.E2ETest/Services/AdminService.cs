using DrivingSchoolApi.DTOs.Common;

namespace DrivingSchoolApi.E2ETest.Services;

public class AdminService
{
    private readonly HttpClient _client;
    private readonly AuthService _authService;
    private JwtTokenDto? Bearer => _authService.Bearer;

    public AdminService(HttpClient client, AuthService authService)
    {
        _client = client;
        _authService = authService;
    }
}