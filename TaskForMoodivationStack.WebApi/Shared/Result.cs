namespace TaskForMoodivationStack.WebApi.Shared;

public class Result<T>
{
    public List<T> Data { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; set; } = new List<Error>();


    public static Result<T> Success(List<T> datas)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = datas
        };
    }
    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = new List<T> { data }
        };
    }

    // Hatalı bir sonuç için
    public static Result<T> Fail(List<Error> errors)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Errors = errors
        };
    }

    // Tek bir hata için
    public static Result<T> Fail(Error error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Errors = new List<Error> { error }
        };
    }
}

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}


