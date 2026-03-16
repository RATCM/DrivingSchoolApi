namespace DrivingSchoolApi.Domain.Primitives;

public record Result {
    public bool IsSuccess { get; }
    public Exception? Error { get; }

    protected Result(bool isSuccess, Exception? error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Exception error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    public static implicit operator Result(Exception error) => Failure(error);
}

public record Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null) => Value = value;
    private Result(Exception error) : base(false, error) { }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Exception error) => new(error);
}

/*
public enum ErrorType { NotFound, Validation, Unauthorized }

public record Error(string Id, ErrorType Type, string Description);

// Error types
public static class Errors {
    public static Error AccountNotFound { get; } = 
        new("AccountNotFound", ErrorType.NotFound, "Account not found.");

    public static Error Unauthorized { get; } =
        new("UnauthorizedAccess", ErrorType.Unauthorized, "Unauthorized. Access denied");
}

*/