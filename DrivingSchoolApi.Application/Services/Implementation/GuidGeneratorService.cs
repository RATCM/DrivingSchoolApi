namespace DrivingSchoolApi.Application.Services.Implementation;

/// <inheritdoc />
internal class GuidGeneratorService : IGuidGeneratorService
{
    public Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}