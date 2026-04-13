namespace DrivingSchoolApi.Application.Exceptions.StudentInvite;

public class StudentInviteNotFoundException : NotFoundException
{
    public StudentInviteNotFoundException(string message) : base(message) { }
}