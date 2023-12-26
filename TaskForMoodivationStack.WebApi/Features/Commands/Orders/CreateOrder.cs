using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using TaskForMoodivationStack.WebApi.Contexts;
using TaskForMoodivationStack.WebApi.Contracts;
using TaskForMoodivationStack.WebApi.Domain.Customers;
using TaskForMoodivationStack.WebApi.Domain.Orders;
using TaskForMoodivationStack.WebApi.Domain.Services;
using TaskForMoodivationStack.WebApi.Domain.ValueObjects;
using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Features.Commands.Orders;

public class CreateOrder
{
    public class Command : IRequest<Result<OrderEntity>>
    {
        public Guid CustomerId { get; set; }
        public string PriceCurrency { get; set; }
        public decimal PriceAmount { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.PriceCurrency).NotEmpty();
            RuleFor(c => c.PriceAmount).NotEmpty();
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IValidator<Command> validator) : IRequestHandler<Command, Result<OrderEntity>>
    {
        public async Task<Result<OrderEntity>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(v =>
                                new Error(v.ErrorCode, v.ErrorMessage)).ToList();

                return Result<OrderEntity>.Fail(errors);
            }
            var orderNumberService = new OrderNumberService(context);
            var order = new OrderEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                OrderNumber = orderNumberService.GetNewOrderNumber(),
                TotalPrice = new Money(request.PriceCurrency,request.PriceAmount),
                CreatedDate = DateTime.Now,
            };
            context.Add(order);
            await context.SaveChangesAsync(cancellationToken);
            return Result<OrderEntity>.Success(order);
        }
    }
}



public class CreateCustomerEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/Orders/CreateOrder", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateOrder.Command>();

            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                var errors = result.Errors.Select(v =>
                                new Error(v.Code, v.Message)).ToList();
                return Results.BadRequest(Result<OrderEntity>.Fail(errors));
            }

            return Results.Ok(Result<OrderEntity>.Success(result.Data));
        });
    }
}
