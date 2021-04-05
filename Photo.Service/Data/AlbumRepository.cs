using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoBank.Photo.Service.Data
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly PhotoServiceDBContext _context;

        public AlbumRepository(PhotoServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        public IEnumerable<AlbumPoco> GetUserAlbums(int userId)
        {
            return _context.Albums.Where(a => a.UserId == userId);
        }

        public void SaveAlbum(AlbumPoco album)
        {
            _context.Add(album);
            _context.SaveChanges();
        }

        public void SaveAlbums(IEnumerable<AlbumPoco> albums)
        {
            foreach (var album in albums)
            {
                _context.Add(album);
            }
            _context.SaveChanges();
        }

        public void DeleteAlbums(IEnumerable<int> albumsId, int userId)
        {
            var albums = _context.Albums.Where(a => a.UserId == userId && albumsId.Contains(a.Id));
            _context.Albums.RemoveRange(albums);
            _context.SaveChanges();
        }
    }
}
