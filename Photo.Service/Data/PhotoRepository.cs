using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoServiceDBContext _context;

        public PhotoRepository(PhotoServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
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
            var photo = new PhotoPoco { UserId = userId, Path = path, Description = "", CreateDate = DateTime.Now };
            _context.Photos.Add(photo);
            _context.SaveChanges();

            return photo.Id;
        }

        public int SavePhoto(PhotoPoco photo)
        {
            _context.Photos.Add(photo);
            _context.SaveChanges();

            return photo.Id;
        }

        public void DeletePhoto(PhotoPoco photo)
        {
            _context.Photos.Remove(photo);
            _context.SaveChanges();
        }

        public void UpdatePhoto(PhotoPoco photo)
        {
            _context.Update(photo);
            _context.SaveChanges();
        }

        public IEnumerable<string> GetAllPhotos()
        {
            return _context.Photos.Select(x => x.Path);
        }
    }
}
