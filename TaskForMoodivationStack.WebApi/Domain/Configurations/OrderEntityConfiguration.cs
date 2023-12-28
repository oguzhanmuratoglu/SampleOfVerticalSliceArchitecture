using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Domain.Entities;

namespace TaskForMoodivationStack.WebApi.Domain.Configurations;

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