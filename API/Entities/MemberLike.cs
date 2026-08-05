namespace API.Entities
{
    public class MemberLike
    {
        public required int SourceMemberId { get; set; }
        public Member SourceMember { get; set; } = null!;

        public required int TargetMemberId { get; set; }
        public Member TargetMember { get; set; } = null!;
    }
}