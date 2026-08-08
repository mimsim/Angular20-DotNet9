using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(string messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
    {
        var query = context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(m => m.RecipientId == messageParams.MemberId && !m.RecipientDeleted),
            "Outbox" => query.Where(m => m.SenderId == messageParams.MemberId && !m.SenderDeleted),
            _ => query.Where(m => m.RecipientId == messageParams.MemberId
                                && m.DateRead == null
                                && !m.RecipientDeleted)
        };

        var totalCount = await query.CountAsync();

        var messages = await query
            .Skip((messageParams.PageNumber - 1) * messageParams.PageSize)
            .Take(messageParams.PageSize)
            .ToListAsync();

        return new PaginatedResult<MessageDto>
        {
            Items = messages.Select(m => m.ToDto()).ToList(),
            CurrentPage = messageParams.PageNumber,
            PageSize = messageParams.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)messageParams.PageSize)
        };
    }
    public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
    {
        var messages = await context.Messages
            .Where(m =>
                (m.SenderId == currentMemberId && m.RecipientId == recipientId && !m.SenderDeleted) ||
                (m.SenderId == recipientId && m.RecipientId == currentMemberId && !m.RecipientDeleted))
                .Include(m => m.Sender)
        .Include(m => m.Recipient)
            .OrderBy(m => m.MessageSent)
            .ToListAsync();

        var unread = messages
            .Where(m => m.DateRead == null && m.RecipientId == currentMemberId)
            .ToList();

        if (unread.Count > 0)
        {
            foreach (var message in unread)
            {
                message.DateRead = DateTime.UtcNow;
            }
            await context.SaveChangesAsync();
        }

        return messages.Select(m => m.ToDto()).ToList();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}