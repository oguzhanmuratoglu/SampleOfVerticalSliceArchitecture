using MediatR;
using TaskForMoodivationStack.WebApi.Shared;
using TaskForMoodivationStack.WebApi.Validation;

namespace TaskForMoodivationStack.WebApi.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultModel, new()
{
    private readonly IValidationHandler<TRequest> _validationHandler;
    public ValidationBehaviour(IValidationHandler<TRequest> validationHandler)
    {
        _validationHandler = validationHandler;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ResultModel result = await _validationHandler.Validate(request);
        if (!result.IsSuccess)
            return new TResponse
            {
                IsSuccess = false,
                Messages = result.Messages
            };
        return await next();
    }
}
