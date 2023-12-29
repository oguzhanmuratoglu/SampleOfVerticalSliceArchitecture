using Corex.Model.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using static TaskForMoodivationStack.WebApi.Features.Commands.Orders.CreateOrder;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public class CreateOrderRequestValidation : AbstractValidator<CreateOrderCommand>, IRequestValidation
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
