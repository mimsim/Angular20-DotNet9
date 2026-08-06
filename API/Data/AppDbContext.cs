using Microsoft.EntityFrameworkCore;
using API;
using API.Entities;
using System.Reflection.Emit;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
public DbSet<AppUser> Users { get; set; }
    // public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Photo> Photos { get; set; }

    public DbSet<MemberLike> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // UserId трябва да е уникален, за да служи като principal key
        builder.Entity<Member>()
            .HasIndex(m => m.UserId)
            .IsUnique();

        builder.Entity<MemberLike>()
            .HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

        builder.Entity<MemberLike>()
            .HasOne(s => s.SourceMember)
            .WithMany(t => t.LikedMembers)
            .HasForeignKey(s => s.SourceMemberId)
            .HasPrincipalKey(m => m.UserId)   // <-- сочи към UserId, не към Id
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MemberLike>()
            .HasOne(s => s.TargetMember)
            .WithMany(t => t.LikedByMembers)
            .HasForeignKey(s => s.TargetMemberId)
            .HasPrincipalKey(m => m.UserId)   // <-- сочи към UserId, не към Id
            .OnDelete(DeleteBehavior.NoAction);
    }
}