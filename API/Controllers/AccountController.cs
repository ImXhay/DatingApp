using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) :BaseApiController
{
    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
{
    if (await UserExists(registerDto.Email)) return BadRequest("Email is taken");
    
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDxo(tokenService); 

    }

    [HttpPost("login")]
public async Task<ActionResult<UserDto>> Login(LoginDTO loginDTO)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginDTO.Email);

        if (user == null) return Unauthorized("Invalid user");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for (var i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");

        }

        return user.ToDxo(tokenService);

    }


private async Task<bool> UserExists(string email)
{
    return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
}
}