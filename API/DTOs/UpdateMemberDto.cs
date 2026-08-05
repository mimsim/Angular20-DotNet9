namespace API.DTOs;

public class UpdateMemberDto
{
    public string? DisplayName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Description { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ImageUrl { get; set; }
}