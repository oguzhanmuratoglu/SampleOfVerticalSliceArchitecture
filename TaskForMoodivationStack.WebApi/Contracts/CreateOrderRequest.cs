namespace TaskForMoodivationStack.WebApi.Contracts;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public string PriceCurrency { get; set; }
    public decimal PriceAmount { get; set; }
}
