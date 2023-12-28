using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskForMoodivationStack.WebApi.Domain.Entities;

namespace TaskForMoodivationStack.WebApi.Domain.Configurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.Property(oi => oi.FirstName).IsRequired();
        builder.Property(oi => oi.LastName).IsRequired();
        builder.Property(oi => oi.Email).IsRequired();
        builder.Property(oi => oi.Password).IsRequired();
        builder.Property(oi => oi.CreatedDate).IsRequired();
    }
}
