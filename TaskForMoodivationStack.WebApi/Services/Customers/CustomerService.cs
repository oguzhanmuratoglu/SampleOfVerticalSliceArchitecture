using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Domain.Entities;
using TaskForMoodivationStack.WebApi.Features.Commands.Customers;

namespace TaskForMoodivationStack.WebApi.Services.Customers;

public class CustomerService(ApplicationDbContext context)
{
    public async Task<RegisterCustomer.Response> AddCustomer(RegisterCustomer.Command request, CancellationToken cancellationToken)
    {
        var customer = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            CreatedDate = DateTime.Now
        };

        context.Add(customer);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterCustomer.Response
        {
            IsSuccess = true,
            Id = customer.Id,
        };
    }
}
