namespace API.Helpers;

public class MessageParams
{
    private const int MaxPageSize = 50;
    private int pageSize = 10;

    public string MemberId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string Container { get; set; } = "Unread"; // "Inbox" | "Outbox" | "Unread"
}