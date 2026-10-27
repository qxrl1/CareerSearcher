using CareerSearcher.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CareerSearcher.Infrastructure.Persistence.Contexts;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
