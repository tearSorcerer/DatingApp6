using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _uow;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork uow)
        {
            _userManager = userManager;
            _uow = uow;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles() 
        {
                var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

                return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles) 
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            var unApprovedPhotos = await _uow.PhotosRepository.GetUnapprovedPhotos();
            return Ok(unApprovedPhotos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            var photo = await _uow.PhotosRepository.GetPhotoById(id);

            if (photo == null) return BadRequest("Photo does not exist");

            if (photo.IsApproved == true) return BadRequest("Photo is already approved");

            photo.IsApproved = true;

            var user = await _uow.UserRepository.GetUserByIdAsync(photo.AppUserId);
            
            if (user == null) return BadRequest("User does not exist");

            if (!user.Photos.Any(p => p.IsMain)) {
                photo.IsMain = true;
            } else {
                photo.IsMain = false;
            }

            if (await _uow.Complete()) return Ok("Approved photo successfully"); 

            return BadRequest("Failed to approve photo");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{id}")] // this should be a http delete, TODO, will change on postman soon...
        public async Task<ActionResult> RejectPhoto(int id)
        {
            var photo = await _uow.PhotosRepository.GetPhotoById(id);

            if (photo == null) return BadRequest("Photo not found");

            if (photo.IsApproved == true) return BadRequest("Photo is already approved, cannot be rejected");

            _uow.PhotosRepository.RemovePhoto(photo);

            if (await _uow.Complete()) return Ok();

            return BadRequest("Failed to reject photo");
        }
    }
}