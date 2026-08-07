using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController(IMessageRepository messageRepository,
    IMemberRepository memberRepository) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var userId = User.GetMemberId();
        if (string.IsNullOrEmpty(userId)) return BadRequest("User not authenticated");

        var sender = await memberRepository.GetMemberByIdAsync(userId);
        var recipient = await memberRepository.GetMemberByIdAsync(createMessageDto.RecipientId);

        if (recipient == null || sender == null || sender.UserId == createMessageDto.RecipientId)
            return BadRequest("Invalid recipient or sender");

        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            SenderId = sender.UserId,
            RecipientId = recipient.UserId,
            Content = createMessageDto.Content,
            Sender = sender,
            Recipient = recipient,
            MessageSent = DateTime.UtcNow
        };

        messageRepository.AddMessage(message);

        if (await messageRepository.SaveAllAsync())
        {
            return Ok(message.ToDto());
        }

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesForMember(
        [FromQuery] MessageParams messageParams)
    {
        var memberId = User.GetMemberId();
        if (string.IsNullOrEmpty(memberId)) return Unauthorized();

        messageParams.MemberId = memberId;
        var result = await messageRepository.GetMessagesForMember(messageParams);
        return Ok(result);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
    {
        var memberId = User.GetMemberId();
        if (string.IsNullOrEmpty(memberId)) return Unauthorized();

        var thread = await messageRepository.GetMessageThread(memberId, recipientId);
        return Ok(thread);
    }
}