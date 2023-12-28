using Corex.Model.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Validations.BusinessValidations;

public class CustomerRegisterBusinessValidation
{
    public async Task<ResultModel> CheckDuplicateEmailAsync(ApplicationDbContext context, string email)
    {
        if (await context.Customers.AnyAsync(c => c.Email == email))
        {
            return ResultModel.Error(new MessageItem
            {
                Code = "DUPLICATE_EMAIL",
                Message = "This email address is already in use. Please try a different email address."
            });
        }
        return ResultModel.Ok();
    }

    public async Task<ResultModel> CheckDuplicateNameAsync(ApplicationDbContext context, string firstName, string lastName)
    {
        if (await context.Customers.AnyAsync(c => c.FirstName == firstName && c.LastName == lastName))
        {
            return ResultModel.Error(new MessageItem
            {
                Code = "DUPLICATE_NAME",
                Message = "A customer with this name and last name already exists. Please provide a unique name."
            });
        }
        return ResultModel.Ok();
    }
}
