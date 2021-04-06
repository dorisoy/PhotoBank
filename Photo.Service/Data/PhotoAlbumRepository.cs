using System.Collections.Generic;
using System.Linq;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoAlbumRepository : IPhotoAlbumRepository
    {
        private readonly PhotoServiceDBContext _context;

        public PhotoAlbumRepository(PhotoServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        public IEnumerable<PhotoAlbumPoco> GetPhotoAlbums(int photoId, int userId)
        {
            return _context.PhotoAlbums.Where(p => p.PhotoId == photoId && p.UserId == userId);
        }

        public void AddPhotoAlbum(int photoId, int albumId, int userId)
        {
            _context.PhotoAlbums.Add(new PhotoAlbumPoco { PhotoId = photoId, AlbumId = albumId, UserId = userId });
            _context.SaveChanges();
        }

        public void AddPhotoAlbums(int photoId, IEnumerable<int> albumsId, int userId)
        {
            foreach (var albumId in albumsId)
            {
                _context.PhotoAlbums.Add(new PhotoAlbumPoco { PhotoId = photoId, AlbumId = albumId, UserId = userId });
            }
            _context.SaveChanges();
        }

        public void DeletePhotoAlbums(int photoId, int userId)
        {
            var photoAlbums = _context.PhotoAlbums.Where(p => p.PhotoId == photoId && p.UserId == userId);
            _context.PhotoAlbums.RemoveRange(photoAlbums);
            _context.SaveChanges();
        }

        public void DeleteAlbumPhotos(IEnumerable<int> albumsId, int userId)
        {
            var albumPhotos = _context.PhotoAlbums.Where(p => albumsId.Contains(p.AlbumId) && p.UserId == userId);
            _context.PhotoAlbums.RemoveRange(albumPhotos);
            _context.SaveChanges();
        }
    }
}
