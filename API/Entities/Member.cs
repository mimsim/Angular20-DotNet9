namespace API.Entities;

public class Member
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public string? ImageUrl { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime LastActive { get; set; } = DateTime.UtcNow;


    public required string Gender { get; set; }

    public string? Description { get; set; }

    public required string City { get; set; }
    public required string Country { get; set; }
    public List<Photo> Photos { get; set; } = [];
    public AppUser User { get; set; } = null!;
}