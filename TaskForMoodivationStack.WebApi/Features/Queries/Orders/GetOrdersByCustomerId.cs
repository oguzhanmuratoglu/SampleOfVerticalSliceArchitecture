using Corex.Model.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Domain.Entities;
using TaskForMoodivationStack.WebApi.Services.Orders;
using TaskForMoodivationStack.WebApi.Shared;
using TaskForMoodivationStack.WebApi.Validation;
using TaskForMoodivationStack.WebApi.Validations.BusinessValidations;
using TaskForMoodivationStack.WebApi.Validations.RequestValidations;

namespace TaskForMoodivationStack.WebApi.Features.Queries.Orders;

public class GetOrdersByCustomerId
{
    public record Query(Guid CustomerId) : IRequest<Response>;
    public class Response : ResultModel
    {
        public List<OrderEntity> Orders { get; set; }
    }


    public class Validator(ApplicationDbContext context) : IValidationHandler<Query>
    {
        public async Task<ResultModel> Validate(Query request)
        {
            var requestValidationRules = new CustomerOrdersRequestValidation();
            var requestValidateResult = requestValidationRules.Validate(request);
            if (!requestValidateResult.IsValid)
            {
                return ResultModel.Error(requestValidationRules.MapValidationErrorsToMessages(requestValidateResult.Errors));
            }

            var businessValidationRules = new CustomerOrdersBusinessValidation();
            var customerValidationResult = await businessValidationRules.CheckCustomerIdAsync(context, request.CustomerId);
            if (!customerValidationResult.IsSuccess)
            {
                return customerValidationResult;
            }
            return ResultModel.Ok();
        }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(ApplicationDbContext context, ILogger<Handler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var orderService = new OrderService(_context);
                return await orderService.GetOrdersByCustomerId(request, cancellationToken);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something went wrong. An error occurred while querying");

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
