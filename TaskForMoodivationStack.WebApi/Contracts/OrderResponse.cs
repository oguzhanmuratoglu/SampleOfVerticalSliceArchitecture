using TaskForMoodivationStack.WebApi.Domain.Customers;
using TaskForMoodivationStack.WebApi.Domain.ValueObjects;

namespace TaskForMoodivationStack.WebApi.Contracts;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string OrderNumber { get; set; }
    public Money TotalPrice { get; set; }
}
