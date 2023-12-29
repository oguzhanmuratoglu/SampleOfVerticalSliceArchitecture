using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Context;
using TaskForMoodivationStack.WebApi.Domain.Entities;
using TaskForMoodivationStack.WebApi.Domain.ValueObjects;
using TaskForMoodivationStack.WebApi.Features.Commands.Orders;
using TaskForMoodivationStack.WebApi.Features.Queries.Orders;
using static TaskForMoodivationStack.WebApi.Features.Commands.Orders.CreateOrder;
using static TaskForMoodivationStack.WebApi.Features.Queries.Orders.GetOrdersByCustomerId;

namespace TaskForMoodivationStack.WebApi.Services.Orders;

public class OrderService(ApplicationDbContext context)
{
    public async Task<GetOrdersByCustomerId.Response> GetOrdersByCustomerId(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders.Where(o=>o.CustomerId == request.CustomerId).ToListAsync(cancellationToken);

        return new GetOrdersByCustomerId.Response
        {
            Orders = orders,
            IsSuccess = true
        };
    }
    public async Task<CreateOrder.Response> AddOrder(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            OrderNumber = GetNewOrderNumber(),
            TotalPrice = new Money(request.PriceCurrency, request.PriceAmount),
            CreatedDate = DateTime.Now
        };

        context.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateOrder.Response
        {
            IsSuccess = true,
            Id = order.Id,
        };
    }

    public string GetNewOrderNumber()
    {
        string initialLetter = "OGZHN";
        string year = DateTime.Now.Year.ToString();
        string newOrderNumber = initialLetter + year;

        var lastOrder = context.Orders.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
        string currentOrderNumber = lastOrder?.OrderNumber;

        if (currentOrderNumber != null)
        {
            string currentYear = currentOrderNumber.Substring(3, 4);
            int startIndex = (currentYear == year) ? 7 : 0;
            GenerateUniqueOrderNumber(ref newOrderNumber, currentOrderNumber.Substring(startIndex));
        }
        else
        {
            newOrderNumber += "000000001";
        }

        return newOrderNumber;
    }

    private void GenerateUniqueOrderNumber(ref string newOrderNumber, string currentOrderNumStr)
    {
        int currentOrderNumberInt = int.TryParse(currentOrderNumStr, out var num) ? num : 0;
        bool isOrderNumberUnique = false;

        while (!isOrderNumberUnique)
        {
            currentOrderNumberInt++;
            newOrderNumber += currentOrderNumberInt.ToString("D9");
            string checkOrderNumber = newOrderNumber;

            var order = context.Orders.FirstOrDefault(o => o.OrderNumber == checkOrderNumber);
            if (order == null)
            {
                isOrderNumberUnique = true;
            }
        }
    }
}
