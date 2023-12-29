using Corex.Model.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using static TaskForMoodivationStack.WebApi.Features.Queries.Orders.GetOrdersByCustomerId;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public class CustomerOrdersRequestValidation : AbstractValidator<GetOrdersByCustomerIdQuery>, IRequestValidation
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

