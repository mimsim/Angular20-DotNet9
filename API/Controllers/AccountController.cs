using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await DisplayNameExists(registerDto.DisplayName))
        {
            return BadRequest("Username is taken");
        }

        if (await EmailExists(registerDto.Email))
        {
            return BadRequest("Email is already registered");
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };

        user.Member = new Member
        {
            UserId = user.Id, // AppUser.Id вече е генериран (обикновено в конструктора/базов клас на Identity)
            DisplayName = registerDto.DisplayName,
            City = string.Empty,
            Country = string.Empty,
            Gender = string.Empty,
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid password");
        }

        return user.ToDto(tokenService);
    }

    private async Task<bool> DisplayNameExists(string displayName)
    {
        return await context.Users.AnyAsync(x =>
            x.DisplayName != null &&
            x.DisplayName.ToLower() == displayName.ToLower());
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x =>
            x.Email != null &&
            x.Email.ToLower() == email.ToLower());
    }
}