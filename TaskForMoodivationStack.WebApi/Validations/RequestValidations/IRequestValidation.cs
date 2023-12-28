using Corex.Model.Infrastructure;
using FluentValidation.Results;

namespace TaskForMoodivationStack.WebApi.Validations.RequestValidations;

public interface IRequestValidation
{
    List<MessageItem> MapValidationErrorsToMessages(IEnumerable<ValidationFailure> validationErrors);
}
