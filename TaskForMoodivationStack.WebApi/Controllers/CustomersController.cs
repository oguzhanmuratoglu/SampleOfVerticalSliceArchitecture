using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskForMoodivationStack.WebApi.Features.Commands.Customers;
using static TaskForMoodivationStack.WebApi.Features.Commands.Customers.RegisterCustomer;

namespace TaskForMoodivationStack.WebApi.Controllers;
public class CustomersController : BaseApiController
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterCustomerCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
