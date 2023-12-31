﻿using Corex.Model.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using static TaskForMoodivationStack.WebApi.Features.Commands.Customers.RegisterCustomer;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public class CustomerRegisterRequestValidation : AbstractValidator<RegisterCustomerCommand>, IRequestValidation
{
    public CustomerRegisterRequestValidation()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).MinimumLength(6).MaximumLength(32).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).MinimumLength(6).MaximumLength(64).NotEmpty();
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
