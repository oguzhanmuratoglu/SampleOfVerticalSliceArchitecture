using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Contexts;
using TaskForMoodivationStack.WebApi.Contracts;
using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Features.Queries.Orders;

public class GetOrdersByCustomerId
{
    public class Query : IRequest<Result<OrderResponse>>
    {
        public Guid CustomerId { get; set;}
    }
    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, Result<OrderResponse>>
    {
        public async Task<Result<OrderResponse>> Handle(Query request, CancellationToken cancellationToken)
        {            
            var order = await context.Orders.Where(o => o.CustomerId == request.CustomerId).Include(p=> p.CustomerEntity).AsNoTracking()
                .Select(o=> new OrderResponse
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderNumber = o.OrderNumber,
                    TotalPrice = o.TotalPrice,
                })
                .ToListAsync(cancellationToken);
            if (order is null)
            {
                return Result<OrderResponse>.Fail(new Error
                (
                    "GetOrdersByCustomerId.Null",
                    "No order found for registered user"
                ));
            }

            return Result<OrderResponse>.Success(order);
        }
    }
}


public class GetOrdersByCustomerIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Orders/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var query = new GetOrdersByCustomerId.Query()
            {
                CustomerId = customerId,
            };

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                var errors = result.Errors.Select(v =>
                                new Error(v.Code, v.Message)).ToList();

                return Results.BadRequest(errors);
            }

            return Results.Ok(result);
        });
    }
}
