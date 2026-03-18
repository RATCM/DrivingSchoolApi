namespace DrivingSchoolApi.Application.Exceptions;

public class ApplicationException : Exception
{
    public int ResponseCode { get; }

    public ApplicationException(int responseCode, string message) : base(message)
    {
        ResponseCode = responseCode;
    }
}