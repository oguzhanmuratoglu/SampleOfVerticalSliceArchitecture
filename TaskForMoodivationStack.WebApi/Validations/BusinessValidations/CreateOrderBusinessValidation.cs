using Corex.Model.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Shared;

namespace TaskForMoodivationStack.WebApi.Validations.BusinessValidations;

public class CreateOrderBusinessValidation
{
    public async Task<ResultModel> CheckCustomerIdAsync(ApplicationDbContext context, Guid customerId)
    {
        var result = await context.Customers.AnyAsync(c => c.Id == customerId);
        if (!result)
        {
            return ResultModel.Error(new MessageItem
            {
                Code = "INVALID_CUSTOMER_ID",
                Message = "The provided customer ID does not exist in the database."
            });
        }
        return ResultModel.Ok();
    }
}
