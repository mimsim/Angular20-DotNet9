using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            var members = await memberRepository.GetMembersAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpGet("{memberId}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string memberId)
        {
            var photos = await memberRepository.GetPhotosForMemberAsync(memberId);
            if (photos == null)
            {
                return NotFound();
            }

            return Ok(photos);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateMember(UpdateMemberDto updateDto)
        {
            // взимаме id-то на логнатия потребител от JWT токена (nameid claim)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var member = await memberRepository.GetMemberForUpdateAsync(userId);

            if (member == null)
            {
                return NotFound();
            }

            // прилагаме само полетата, които реално са подадени (null = не се променя)
            member.DisplayName = updateDto.DisplayName ?? member.DisplayName;
            member.DateOfBirth = updateDto.DateOfBirth ?? member.DateOfBirth;
            member.Gender = updateDto.Gender ?? member.Gender;
            member.Description = updateDto.Description ?? member.Description;
            member.City = updateDto.City ?? member.City;
            member.Country = updateDto.Country ?? member.Country;
            member.ImageUrl = updateDto.ImageUrl ?? member.ImageUrl;
            member.User.DisplayName = updateDto.DisplayName ?? member.User.DisplayName;
            memberRepository.Update(member);

            if (await memberRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Неуспешно запазване на промените.");
        }
    }
}