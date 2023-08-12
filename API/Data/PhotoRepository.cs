using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int id)
        {   
            var photo = await _context.Photos
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            var queryable = _context.Photos
                    .Include(u => u.AppUser)
                    .Where(photo => photo.IsApproved == false )
                    .IgnoreQueryFilters()
                    .Select(photo => new PhotoForApprovalDto {
                        Username = photo.AppUser.UserName,
                        IsApproved = photo.IsApproved,
                        Id = photo.Id,
                        Url = photo.Url
                    });

            return await queryable.ToListAsync();
        }
    }
}