namespace DrivingSchoolApi.Application.Exceptions.StudentInvite;

// This should probably not be a bad request
public class StudentInviteExpiredException : BadRequestException
{
    public StudentInviteExpiredException(string message) : base(message) { }
}