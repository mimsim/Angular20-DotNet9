using Microsoft.EntityFrameworkCore;
using API;
using API.Entities;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(mem => mem.MessagesSent)
            .HasForeignKey(m => m.SenderId)
            .HasPrincipalKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(mem => mem.MessagesReceived)
            .HasForeignKey(m => m.RecipientId)
            .HasPrincipalKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Member>()
            .HasIndex(m => m.UserId)
            .IsUnique();

        builder.Entity<MemberLike>()
            .HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

        builder.Entity<MemberLike>()
            .HasOne(s => s.SourceMember)
            .WithMany(t => t.LikedMembers)
            .HasForeignKey(s => s.SourceMemberId)
            .HasPrincipalKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );
        var nulableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : null,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
        );

        builder.Entity<MemberLike>()
            .HasOne(s => s.TargetMember)
            .WithMany(t => t.LikedByMembers)
            .HasForeignKey(s => s.TargetMemberId)
            .HasPrincipalKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}