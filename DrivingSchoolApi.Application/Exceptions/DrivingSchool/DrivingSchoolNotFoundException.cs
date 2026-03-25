namespace DrivingSchoolApi.Application.Exceptions.DrivingSchool;

public class DrivingSchoolNotFoundException : NotFoundException
{
    public DrivingSchoolNotFoundException(string message) : base(message) { }
}