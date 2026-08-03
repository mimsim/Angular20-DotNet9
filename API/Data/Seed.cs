using System.Security.Cryptography;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var membersData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(membersData);

        if (members == null)
        {
            return;
        }

        using var hmac = new HMACSHA512();
        foreach (var member in members)
        {
            var user = new AppUser
            {
                Id = member.UserId,
                DisplayName = member.DisplayName,
                Email = member.Email,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Pa$$w0rd")), // Placeholder for password hash
                PasswordSalt = hmac.Key, // Placeholder for password salt
                Member = new Member
                {
                    UserId = member.UserId,
                    DateOfBirth = member.DateOfBirth,
                    Gender = member.Gender,
                    Description = member.Description,
                    City = member.City,
                    Country = member.Country,
                    Created = member.Created,
                }
            };
            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                PublicId = $"seed-{member.UserId}",
                MemberId = member.UserId,
            });
            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}