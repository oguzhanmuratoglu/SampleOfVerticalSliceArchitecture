using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Validation;

public interface IValidationHandler
{
}
public interface IValidationHandler<T> : IValidationHandler
{
    Task<ResultModel> Validate(T request);
}
