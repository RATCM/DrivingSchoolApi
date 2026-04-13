namespace DrivingSchoolApi.Application.Services.Implementation;

public class DateTimeProviderService : IDateTimeProviderService
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}