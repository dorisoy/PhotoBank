using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoServiceDBContext _context;

        public PhotoRepository(PhotoServiceDBContext context)
        {
            _context = context;
        }

        public IEnumerable<PhotoPoco> GetPhotos(int userId)
        {
            return Enumerable.Empty<PhotoPoco>();
        }
    }
}
