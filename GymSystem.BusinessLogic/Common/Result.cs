namespace GymSystem.BusinessLogic.Common;

public class Result
{
    protected Result(bool isSuccess, ResultStatus status, string? error)
    {
        IsSuccess = isSuccess;
        Status = status;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public string? Error { get; }

    public ResultStatus Status { get; }

    public static Result Ok() => new(true, ResultStatus.Success, null);
    public static Result<T> Ok<T>(T value) => Result<T>.Success(value);

    public static Result Fail(string error, ResultStatus status = ResultStatus.Invalid)
        => new(false, status, error);

    public static Result NotFound(string error = "The requested item was not found.")
        => new(false, ResultStatus.NotFound, error);

    public static Result Conflict(string error)
        => new(false, ResultStatus.Conflict, error);

    public static Result<T> Fail<T>(string error, ResultStatus status = ResultStatus.Invalid)
        => Result<T>.Failure(error, status);

    public static Result<T> NotFound<T>(string error = "The requested item was not found.")
        => Result<T>.Failure(error, ResultStatus.NotFound);

    public static Result<T> Conflict<T>(string error)
        => Result<T>.Failure(error, ResultStatus.Conflict);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(bool isSuccess, ResultStatus status, string? error, T? value)
        : base(isSuccess, status, error)
        => _value = value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            $"Cannot read Value of a failed result ({Status}): {Error}");

    internal static Result<T> Success(T value) => new(true, ResultStatus.Success, null, value);

    internal static Result<T> Failure(string error, ResultStatus status)
        => new(false, status, error, default);

    public static implicit operator Result<T>(T value) => Success(value);
}
