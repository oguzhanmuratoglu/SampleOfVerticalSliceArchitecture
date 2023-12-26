
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TaskForMoodivationStack.WebApi.Domain.Customers;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
}

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
