using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using TaskForMoodivationStack.WebApi.Contexts;
using TaskForMoodivationStack.WebApi.Contracts;
using TaskForMoodivationStack.WebApi.Domain.Customers;
using TaskForMoodivationStack.WebApi.Shared;
using Error = TaskForMoodivationStack.WebApi.Shared.Error;


namespace TaskForMoodivationStack.WebApi.Features.Commands.Customers;

public class RegisterCustomer
{
    public class Command : IRequest<Result<CustomerEntity>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c=>c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c=>c.Email).MinimumLength(6).MaximumLength(32).NotEmpty().EmailAddress();
            RuleFor(c=>c.Password).MinimumLength(8).MaximumLength(64).NotEmpty();

        }
    }

    internal sealed class Handler(ApplicationDbContext context, IValidator<Command> validator) : IRequestHandler<Command, Result<CustomerEntity>>
    {
        public async Task<Result<CustomerEntity>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(v =>
                                new Error(v.ErrorCode, v.ErrorMessage)).ToList();

                return Result<CustomerEntity>.Fail(errors);
            }
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
            return Result<CustomerEntity>.Success(customer);
        }
    }
}



public class CreateCustomerEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/Customers/RegisterCustomer", async (RegisterCustomerRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCustomer.Command>();

            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                var errors = result.Errors.Select(v =>
                                new Error(v.Code, v.Message)).ToList();

                return Results.BadRequest(Result<CustomerEntity>.Fail(errors));
            }

            return Results.Ok(Result<CustomerEntity>.Success(result.Data));
        });
    }
}
