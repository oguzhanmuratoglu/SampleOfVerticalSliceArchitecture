using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskForMoodivationStack.WebApi.Domain.Customers;
using TaskForMoodivationStack.WebApi.Domain.Orders;

namespace TaskForMoodivationStack.WebApi.Contexts;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
