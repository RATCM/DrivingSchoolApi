namespace DrivingSchoolApi.Application.Services;

/// <summary>
/// Generates random GUIDs
/// </summary>
/// <remarks>
/// This is useful for when we need to have
/// more control in tests
/// </remarks>
public interface IGuidGeneratorService
{
    Guid NewGuid();
}