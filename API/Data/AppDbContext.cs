using Microsoft.EntityFrameworkCore;
using API;
using API.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
public DbSet<AppUser> Users { get; set; }
    // public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}