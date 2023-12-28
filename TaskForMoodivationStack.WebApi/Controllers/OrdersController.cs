using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskForMoodivationStack.WebApi.Features.Commands.Orders;
using TaskForMoodivationStack.WebApi.Features.Queries.Orders;

namespace TaskForMoodivationStack.WebApi.Controllers;
public class OrdersController(IMediator mediator) : BaseApiController
{
    [HttpPost]
    public async Task<CreateOrder.Response> AddAsync(CreateOrder.Command request)
    {
        return await mediator.Send(request);
    }

    [HttpGet("{customerId}")]
    public async Task<GetOrdersByCustomerId.Response> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var request = new GetOrdersByCustomerId.Query(customerId);
        return await mediator.Send(request);
    }
}
