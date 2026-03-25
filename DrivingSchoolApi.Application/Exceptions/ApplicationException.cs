namespace DrivingSchoolApi.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    public int ResponseCode { get; }

    public ApplicationException(int responseCode, string message) : base(message)
    {
        ResponseCode = responseCode;
    }
}