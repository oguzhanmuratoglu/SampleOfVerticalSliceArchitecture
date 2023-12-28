using Corex.Model.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using TaskForMoodivationStack.WebApi.Features.Commands.Customers;
using TaskForMoodivationStack.WebApi.Features.Commands.Orders;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public class CreateOrderRequestValidation : AbstractValidator<CreateOrder.Command>, IRequestValidation
{
    public CreateOrderRequestValidation()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.PriceCurrency).NotEmpty();
        RuleFor(c => c.PriceAmount).NotEmpty();
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
