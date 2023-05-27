using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        private IUserRepository _userRepository;

        public AccountController(DataContext context, ITokenService TokenService, IUserRepository userRepository)
        {
            _tokenService = TokenService;
            _context = context;
            _userRepository = userRepository;

        }

        [HttpPost("register")] //POST // api/Account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("Username already exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();



            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(users => users.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }

            // var currentUser = await _userRepository.GetMemberAsync(user.UserName);
            

            //var userObj = currentUser.Photos.FirstOrDefault(x => x.IsMain)?.Url;
            //JsonConvert.SerializeObject().ToString();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                // userObj
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync
                (user => user.UserName == username.ToLower());
            //  return await _context.Users.AnyAsync(user => string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase));
        }

    }
}