using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikesRepository likesRepository) : BaseApiController
{
    [HttpPost("{targetMemberId}")]
    public async Task<ActionResult> ToggleLike(string targetMemberId)
    {
        var sourceMemberId = User.GetMemberId();

        if (string.IsNullOrEmpty(sourceMemberId))
            return Unauthorized();

        if (sourceMemberId == targetMemberId)
            return BadRequest("You cannot like yourself.");

        var existingLike = await likesRepository.GetMemberLikeAsync(sourceMemberId, targetMemberId);

        if (existingLike == null)
        {
            var like = new MemberLike
            {
                SourceMemberId = sourceMemberId,
                TargetMemberId = targetMemberId
            };

            likesRepository.AddLike(like);

            if (await likesRepository.SaveAllChangesAsync())
                return Ok();

            return BadRequest("Failed to like member.");
        }
        else
        {
            likesRepository.DeleteLike(existingLike);

            if (await likesRepository.SaveAllChangesAsync())
                return Ok();

            return BadRequest("Failed to unlike member.");
        }
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikeIds()
    {
        var memberId = User.GetMemberId();
        if (string.IsNullOrEmpty(memberId))
            return Unauthorized();

        return Ok(await likesRepository.GetCurrentMemberLikeIds(memberId));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Member>>> GetMembersLikes(string predicate)
    {
        var memberId = User.GetMemberId();
        if (string.IsNullOrEmpty(memberId))
            return Unauthorized();

        var members = await likesRepository.GetMembersLikes(predicate, memberId);
        return Ok(members);
    }
}