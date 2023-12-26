using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using TaskForMoodivationStack.WebApi.Contexts;
using TaskForMoodivationStack.WebApi.Domain.Customers;
using TaskForMoodivationStack.WebApi.Domain.Services;
using TaskForMoodivationStack.WebApi.Domain.ValueObjects;
using System.Reflection.Emit;

namespace TaskForMoodivationStack.WebApi.Domain.Orders;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerEntity CustomerEntity { get; set; }
    public string OrderNumber { get; set; }
    public Money TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.Property(oi => oi.CustomerId).IsRequired();
        builder.Property(oi => oi.OrderNumber).IsRequired();
        builder.Property(oi => oi.CreatedDate).IsRequired();
        builder.OwnsOne(p => p.TotalPrice, price =>
        {
            price.Property(p => p.Currency).HasMaxLength(5).IsRequired();
            price.Property(p => p.Amount).IsRequired();
        });
    }
}
