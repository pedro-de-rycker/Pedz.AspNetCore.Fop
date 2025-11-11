using Microsoft.EntityFrameworkCore;
using WebApiExample.Entities;

namespace WebApiExample.Persistence;

public class FooDbContext(
    DbContextOptions<FooDbContext> options)
    : DbContext(options)
{
    public DbSet<FooEntity> Foos { get; set; }
}
