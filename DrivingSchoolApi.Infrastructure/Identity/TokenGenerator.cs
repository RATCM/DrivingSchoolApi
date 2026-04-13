using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DrivingSchoolApi.Application.Enums;
using DrivingSchoolApi.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DrivingSchoolApi.Infrastructure.Identity;

internal class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IGuidGeneratorService _guidGenerator;
    private readonly IConfiguration _configuration;

    public TokenGeneratorService(IGuidGeneratorService guidGenerator, IConfiguration configuration)
    {
        _guidGenerator = guidGenerator;
        _configuration = configuration;
    }

    public string GenerateJwtAccessToken(Guid userId, UserRole role)
    {
        var scheme = _configuration.GetSection("Authentication:Schemes:Access");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("role", role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, _guidGenerator.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Convert.FromBase64String(
            scheme["SigningKeys:0:Value"]!));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            issuer: scheme["ValidIssuer"]!,
            audience: scheme["Audience"]!,
            claims: claims,
            expires: DateTime.Now.Add(TimeSpan.Parse(scheme["TimeValid"]!)),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateJwtRefreshToken(Guid userId, UserRole role)
    {
        var scheme = _configuration.GetSection("Authentication:Schemes:Refresh");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("role", role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, _guidGenerator.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Convert.FromBase64String(
            scheme["SigningKeys:0:Value"]!));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            issuer: scheme["ValidIssuer"]!,
            audience: scheme["Audience"]!,
            claims: claims,
            expires: DateTime.Now.Add(TimeSpan.Parse(scheme["TimeValid"]!)),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}