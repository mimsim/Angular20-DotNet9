using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.DisplayName))
        {
            return BadRequest("Username is taken");
        }

        if (await UserExistsByEmail(registerDto.Email))
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
            Member = new Member
            {
                UserId = string.Empty, // ще се сетне по-долу, след като имаме user.Id
                DisplayName = registerDto.DisplayName,
                City = string.Empty,
                Country = string.Empty,
                Gender = string.Empty,
            }
        };

        // UserId на Member трябва да съвпада с AppUser.Id (string GUID)
        user.Member.UserId = user.Id;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x =>
            x.DisplayName != null &&
            x.DisplayName.ToLower() == username.ToLower());
    }

    private async Task<bool> UserExistsByEmail(string email)
    {
        return await context.Users.AnyAsync(x =>
            x.Email != null &&
            x.Email.ToLower() == email.ToLower());
    }

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

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return user.ToDto(tokenService);
    }
}