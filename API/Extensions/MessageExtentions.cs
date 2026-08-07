using API.DTOs;
using API.Entities;

namespace API.Extensions
{
    public static class MessageExtentions
    {
        public static MessageDto ToDto(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderDisplayName = message.Sender.DisplayName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientId = message.RecipientId.ToString(),
                RecipientDisplayName = message.Recipient.DisplayName,
                RecipientImageUrl = message.Recipient.ImageUrl,
                Content = message.Content
            };
        }
    }
}