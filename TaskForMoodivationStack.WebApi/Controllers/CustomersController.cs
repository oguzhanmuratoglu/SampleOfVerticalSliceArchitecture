using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskForMoodivationStack.WebApi.Features.Commands.Customers;

namespace TaskForMoodivationStack.WebApi.Controllers;
public class CustomersController(IMediator mediator) : BaseApiController
{
    [HttpPost]
    public async Task<RegisterCustomer.Response> Register(RegisterCustomer.Command request)
    {
        return await mediator.Send(request);
    }
}
