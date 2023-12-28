using TaskForMoodivationStack.WebApi.Domain.ValueObjects;

namespace TaskForMoodivationStack.WebApi.Domain.Entities;

public class OrderEntity : BaseEntity
{
    public Guid CustomerId { get; set; }
    public CustomerEntity? CustomerEntity { get; set; }
    public string OrderNumber { get; set; }
    public Money TotalPrice { get; set; }
}


