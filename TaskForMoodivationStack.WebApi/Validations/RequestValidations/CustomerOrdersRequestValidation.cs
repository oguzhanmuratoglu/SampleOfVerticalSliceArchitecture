using Corex.Model.Infrastructure;
using FluentValidation.Results;
using FluentValidation;
using TaskForMoodivationStack.WebApi.Features.Commands.Orders;
using TaskForMoodivationStack.WebApi.Features.Queries.Orders;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public class CustomerOrdersRequestValidation : AbstractValidator<GetOrdersByCustomerId.Query>, IRequestValidation
{
    public CustomerOrdersRequestValidation()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }

    public List<MessageItem> MapValidationErrorsToMessages(IEnumerable<ValidationFailure> validationErrors)
    {
        return validationErrors.Select(v => new MessageItem
        {
            Code = v.ErrorCode,
            Message = v.ErrorMessage,
        }).ToList();
    }
}

