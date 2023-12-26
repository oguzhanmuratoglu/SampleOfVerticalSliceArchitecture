namespace TaskForMoodivationStack.WebApi.Domain.ValueObjects;

public sealed class Money //ValueObject
{
    public Money(string currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public string Currency { get; private init; }
    public decimal Amount { get; private init; }
}
