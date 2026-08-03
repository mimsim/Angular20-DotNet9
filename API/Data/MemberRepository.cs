using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
  public async Task<Member?> GetMemberByIdAsync(int id)
  {
    return await context.Members.FindAsync(id);
  }

    public Task<Member?> GetMemberByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
  {
    return await context.Members
    .Include(m => m.Photos)
    .ToListAsync();
  }

    public Task<IReadOnlyList<Photo>> GetMembersForMemberAsync(string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(int memberId)
  {
    return await context.Members
    .Where(m => m.Id == memberId)
    .SelectMany(m => m.Photos)
    .ToListAsync();
  }

  public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
  {
    if (!int.TryParse(memberId, out var parsedMemberId))
    {
      return new List<Photo>();
    }

    return await context.Members
      .Where(m => m.Id == parsedMemberId)
      .SelectMany(m => m.Photos)
      .ToListAsync();
  }

  public async Task<bool> SaveAllAsync()
  {
    return await context.SaveChangesAsync() > 0;
  }

  public void Update(Member member)
  {
    context.Entry(member).State = EntityState.Modified;
  }
}
