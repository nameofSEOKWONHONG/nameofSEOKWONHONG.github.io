using Microsoft.EntityFrameworkCore;

namespace MudExample.Data;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Id).ValueGeneratedOnAdd();
        });
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}