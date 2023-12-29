using Corex.Model.Infrastructure;
using FluentValidation;
using MediatR;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Services.Customers;
using TaskForMoodivationStack.WebApi.Shared;
using TaskForMoodivationStack.WebApi.Validation;
using TaskForMoodivationStack.WebApi.Validations.BusinessValidations;
using TaskForMoodivationStack.WebApi.Validations.RequestValidations;


namespace TaskForMoodivationStack.WebApi.Features.Commands.Customers;

public class RegisterCustomer
{
    public record RegisterCustomerCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Response>;
    public class Response : ResultModel
    {
        public Guid Id { get; set; }
    }


    public class Validator(ApplicationDbContext context) : IValidationHandler<RegisterCustomerCommand>
    {
        public async Task<ResultModel> Validate(RegisterCustomerCommand request)
        {
            var requestValidationRules = new CustomerRegisterRequestValidation();
            var requestValidateResult = requestValidationRules.Validate(request);
            if (!requestValidateResult.IsValid)
            {
                return ResultModel.Error(requestValidationRules.MapValidationErrorsToMessages(requestValidateResult.Errors));
            }

            var businessValidationRules = new CustomerRegisterBusinessValidation();
            var emailValidationResult = await businessValidationRules.CheckDuplicateEmailAsync(context, request.Email);
            if (!emailValidationResult.IsSuccess)
            {
                return emailValidationResult;
            }
            var nameValidationResult = await businessValidationRules.CheckDuplicateNameAsync(context, request.FirstName, request.LastName);
            if (!nameValidationResult.IsSuccess)
            {
                return nameValidationResult;
            }
            return ResultModel.Ok();
        }
    }

    internal sealed class Handler : IRequestHandler<RegisterCustomerCommand, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(ApplicationDbContext context, ILogger<Handler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerService = new CustomerService(_context);
                return await customerService.AddCustomer(request, cancellationToken);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something went wrong. There was a problem during customer registration.");

                return new Response
                {
                    IsSuccess = false,
                    Messages = new List<MessageItem>
                    {
                        new MessageItem
                        {
                            Code = $"DB_ERROR_00{ex.HResult}",
                            Message = "An error occurred while processing your request. Please try again later."
                        }
                    }
                };
            }

        }
    }
}
