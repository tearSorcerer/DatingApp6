using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public IUserRepository _userRepository;
        public IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this._mapper = mapper;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
           var users = await _userRepository.GetMembersAsync();
           
           return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRepository.GetMemberAsync(username);
        }
    }
}