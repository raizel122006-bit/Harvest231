using Harvest231.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvest231.Api.Data;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Farm> Farms { get; set; }

}