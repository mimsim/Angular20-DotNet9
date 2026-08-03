using System.Text.Json.Serialization;

namespace API.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public required string PublicId { get; set; }
    [JsonIgnore]
    public Member Member { get; set; } = null!;
    public int MemberId { get; set; }
}