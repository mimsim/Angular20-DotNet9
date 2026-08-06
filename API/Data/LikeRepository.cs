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

    public async Task<bool> SaveAllChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
            .Where(like => like.SourceMemberId == memberId)
            .Select(like => like.TargetMemberId)
            .ToListAsync();
    }

    public Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId)
    {
        return context.Likes.FindAsync(sourceMemberId, targetMemberId).AsTask();
    }

    public async Task<IReadOnlyList<Member>> GetMembersLikes(string predicate, string memberId)
    {
        var query = context.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                return await query.Where(like => like.SourceMemberId == memberId)
                    .Select(like => like.TargetMember)
                    .ToListAsync();

            case "likedBy":
                return await query.Where(like => like.TargetMemberId == memberId)
                    .Select(like => like.SourceMember)
                    .ToListAsync();

            default:
                return await query
                    .Where(like => like.SourceMemberId == memberId || like.TargetMemberId == memberId)
                    .Select(like => like.SourceMemberId == memberId ? like.TargetMember : like.SourceMember)
                    .ToListAsync();
        }
    }

    public async Task<IReadOnlyList<string>> GetUserLikesAsync(string memberId)
    {
        return await context.Likes
            .Where(like => like.SourceMemberId == memberId)
            .Select(like => like.TargetMemberId)
            .ToListAsync();
    }
}