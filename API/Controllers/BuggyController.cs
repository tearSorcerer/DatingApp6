using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context) 
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() 
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() 
        {
            var userThatDoesntExist = _context.Users.Find(-1);

            if (userThatDoesntExist == null) return NotFound();

            return userThatDoesntExist;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() 
        {

            var userThatDoesntExist = _context.Users.Find(-1);

            var userThatDoesntExistReturn = userThatDoesntExist.ToString();

            return userThatDoesntExistReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() 
        {
            return BadRequest("This was not a good request");
        }
        
    }
}