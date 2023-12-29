using MediatR;
using Microsoft.AspNetCore.Mvc;
using static TaskForMoodivationStack.WebApi.Features.Commands.Orders.CreateOrder;
using static TaskForMoodivationStack.WebApi.Features.Queries.Orders.GetOrdersByCustomerId;

namespace TaskForMoodivationStack.WebApi.Controllers;
public class OrdersController : BaseApiController
{
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> AddAsync(CreateOrderCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var request = new GetOrdersByCustomerIdQuery(customerId);
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
