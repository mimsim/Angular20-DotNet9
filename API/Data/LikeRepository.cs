using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;


public class LikeRepository(AppDbContext context) : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>?> GetCurrentMemberLikeIds(string memberId)
    {
        var memberIdInt = int.Parse(memberId);

        var likedMemberIds = await context.Likes
            .Where(like => like.SourceMemberId == memberIdInt)
            .Select(like => like.TargetMemberId.ToString())
            .ToListAsync();

        return likedMemberIds;
    }

    public Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Member>> GetMembersLikes(string predicate, string memberId)
    {
        var query = context.Likes.AsQueryable();
        switch (predicate)
        {
            case "liked":
                query = query.Where(like => like.SourceMemberId.ToString() == memberId);
                break;
            case "likedBy":
                query = query.Where(like => like.TargetMemberId.ToString() == memberId);
                break;
            default:
                var likeIds = await GetCurrentMemberLikeIds(memberId);
              return await query.Where(like => like.SourceMemberId.ToString() == memberId || like.TargetMemberId.ToString() == memberId)
                    .Select(like => like.SourceMemberId.ToString() == memberId ? like.TargetMember : like.SourceMember)
                    .ToListAsync();

        }

        return new List<Member>();
    }

    public Task<IReadOnlyList<string>> GetUserLikesAsync(string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}