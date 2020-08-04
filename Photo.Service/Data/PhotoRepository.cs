using System.Collections.Generic;
using System.Linq;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoServiceDBContext _context;

        public PhotoRepository(PhotoServiceDBContext context)
        {
            _context = context;
        }

        public IEnumerable<PhotoPoco> GetUserPhotos(int userId)
        {
            return _context.Photos.Where(x => x.UserId == userId).ToList();
        }

        public PhotoPoco GetPhoto(int photoId)
        {
            return _context.Photos.FirstOrDefault(x => x.Id == photoId);
        }

        public int SavePhoto(int userId, string path)
        {
            var poco = new PhotoPoco { UserId = userId, Path = path };
            _context.Photos.Add(poco);
            _context.SaveChanges();

            return poco.Id;
        }
    }
}
