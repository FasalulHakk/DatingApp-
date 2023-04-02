using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataConetxt _conetxt;
        private readonly ITokenService _tokenService;
        public AccountController(DataConetxt conetxt, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _conetxt = conetxt;
            
        }

        [HttpPost("register")]  // POST : /api/account/register?username=dav&password=pwd
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            
            if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _conetxt.Users.Add(user);
            await _conetxt.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user = await _conetxt.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if(user ==null) return Unauthorized("invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };

        }

        private async Task<bool>UserExists(string username)
        {
            return await _conetxt.Users.AnyAsync(x => x.UserName == username.ToLower()); // here the 'x' is as  a user from 'Users'.
        }
        
    }
}