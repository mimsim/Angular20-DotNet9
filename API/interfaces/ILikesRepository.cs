using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId);
        Task<IReadOnlyList<Member>> GetMembersLikes(string predicate, string memberId);
        Task<IReadOnlyList<string>> GetUserLikesAsync(string memberId);

        void DeleteLike(MemberLike like);
        void AddLike(MemberLike like);
        Task<bool> SaveAllChangesAsync();
    }
}