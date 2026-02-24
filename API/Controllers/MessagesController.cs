using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// [Authorize]
public class MessagesController(IMessageRepository messageRepository, IMemberRepository memberRepository) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await memberRepository.GetMembersByIdAsync(User.GetMemberId());
        var recipient = await memberRepository.GetMembersByIdAsync(createMessageDto.RecipientId);

        if (recipient == null || sender == null || sender.Id == createMessageDto.RecipientId)
            return BadRequest("Oops! Something went wrong. We couldn't send your message.");
        
        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = createMessageDto.Content
        };

        messageRepository.AddMessage(message);

        if (await messageRepository.SaveAllAsync()) return message.ToDto();

        return BadRequest("Failed to send message");
    }


[HttpGet]

public async Task<ActionResult<PaginatedResult<MessageDTO>>> GetMessagesByContainer( [FromQuery] MessageParams messageParams)
    {
        messageParams.MemberId = User.GetMemberId();

        return await messageRepository.GetMessageForMember(messageParams);
    }

[HttpGet("thread/{recipientId}")]
public async Task<ActionResult<IReadOnlyList<MessageDTO>>> GetMessageThread(string recipientId)
    {
        return Ok(await messageRepository.GetMessagesThread(User.GetMemberId(), recipientId));
    }

[HttpDelete("{id}")]
public async Task<ActionResult> DeleteMessage(string id)
    {
        var memberId = User.GetMemberId();

        var message =await messageRepository.GetMessage(id);

        if ( message == null ) return BadRequest("Cannot delete this message");

        if (message.SenderId != memberId && message.RecipientId != memberId) return BadRequest("You cannot delete this message");
        
        if (message.SenderId == memberId) message.SenderDeleted=true;
        if (message.RecipientId == memberId) message.RecipientDeleted=true;

        if (message is {SenderDeleted: true, RecipientDeleted: true })
        {
            messageRepository.DeleteMessage(message);
        }

        if (await messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the message");

    }

}