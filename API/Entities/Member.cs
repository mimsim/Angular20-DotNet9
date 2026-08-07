using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public List<Photo> Photos { get; set; } = [];
    public List<MemberLike> LikedByMembers { get; set; } = [];
    [JsonIgnore]
    public List<MemberLike> LikedMembers { get; set; } = [];
    [JsonIgnore]
    public List<Message> MessagesSent { get; set; } = [];
    [JsonIgnore]
    public List<Message> MessagesReceived { get; set; } = [];
    [JsonIgnore]
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;
}