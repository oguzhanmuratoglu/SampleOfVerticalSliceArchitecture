using Corex.Model.Infrastructure;
using MediatR;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Services.Customers;
using TaskForMoodivationStack.WebApi.Services.Orders;
using TaskForMoodivationStack.WebApi.Shared;
using TaskForMoodivationStack.WebApi.Validation;
using TaskForMoodivationStack.WebApi.Validations.BusinessValidations;
using TaskForMoodivationStack.WebApi.Validations.RequestValidations;

namespace TaskForMoodivationStack.WebApi.Features.Commands.Orders;

public class CreateOrder
{
    public record CreateOrderCommand(Guid CustomerId, string PriceCurrency, decimal PriceAmount) : IRequest<Response>;
    public class Response : ResultModel
    {
        public Guid Id { get; set; }
    }


    public class Validator(ApplicationDbContext context) : IValidationHandler<CreateOrderCommand>
    {
        public async Task<ResultModel> Validate(CreateOrderCommand request)
        {
            var requestValidationRules = new CreateOrderRequestValidation();
            var requestValidateResult = requestValidationRules.Validate(request);
            if (!requestValidateResult.IsValid)
            {
                return ResultModel.Error(requestValidationRules.MapValidationErrorsToMessages(requestValidateResult.Errors));
            }

            var businessValidationRules = new CreateOrderBusinessValidation();
            var customerValidationResult = await businessValidationRules.CheckCustomerIdAsync(context, request.CustomerId);
            if (!customerValidationResult.IsSuccess)
            {
                return customerValidationResult;
            }
            return ResultModel.Ok();
        }
    }

    internal sealed class Handler : IRequestHandler<CreateOrderCommand, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(ApplicationDbContext context, ILogger<Handler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderService = new OrderService(_context);
                return await orderService.AddOrder(request, cancellationToken);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something went wrong. There was an error while creating the order");

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
