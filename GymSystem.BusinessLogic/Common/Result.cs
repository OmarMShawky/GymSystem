namespace GymSystem.BusinessLogic.Common;

/// <summary>
/// Encodes success or failure as a return value instead of an exception or a null.
/// Callers must check <see cref="IsSuccess"/> before using a result.
/// </summary>
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

    /// <summary>Why it failed. Null on success.</summary>
    public string? Error { get; }

    public ResultStatus Status { get; }

    //----- success -----
    public static Result Ok() => new(true, ResultStatus.Success, null);
    public static Result<T> Ok<T>(T value) => Result<T>.Success(value);

    //----- failure -----
    public static Result Fail(string error, ResultStatus status = ResultStatus.Invalid)
        => new(false, status, error);

    public static Result NotFound(string error = "The requested item was not found.")
        => new(false, ResultStatus.NotFound, error);

    public static Result Conflict(string error)
        => new(false, ResultStatus.Conflict, error);

    //----- typed failure (carries no value) -----
    public static Result<T> Fail<T>(string error, ResultStatus status = ResultStatus.Invalid)
        => Result<T>.Failure(error, status);

    public static Result<T> NotFound<T>(string error = "The requested item was not found.")
        => Result<T>.Failure(error, ResultStatus.NotFound);

    public static Result<T> Conflict<T>(string error)
        => Result<T>.Failure(error, ResultStatus.Conflict);
}

/// <summary>A <see cref="Result"/> that carries a value when successful.</summary>
public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(bool isSuccess, ResultStatus status, string? error, T? value)
        : base(isSuccess, status, error)
        => _value = value;

    /// <summary>The value. Throws if the result is a failure - check IsSuccess first.</summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            $"Cannot read Value of a failed result ({Status}): {Error}");

    internal static Result<T> Success(T value) => new(true, ResultStatus.Success, null, value);

    internal static Result<T> Failure(string error, ResultStatus status)
        => new(false, status, error, default);

    /// <summary>Lets a successful value be returned directly: <c>return viewModel;</c></summary>
    public static implicit operator Result<T>(T value) => Success(value);
}
