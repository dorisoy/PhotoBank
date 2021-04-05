using System.Collections.Generic;
using PhotoBank.DataAccess;

namespace PhotoBank.Photo.Service.Data
{
    public interface IPhotoAlbumRepository : IRepository
    {
        IEnumerable<PhotoAlbumPoco> GetPhotoAlbums(int photoId, int userId);

        void AddPhotoAlbum(int photoId, int albumId, int userId);

        void AddPhotoAlbums(int photoId, IEnumerable<int> albumsId, int userId);

        void DeletePhotoAlbums(int photoId, int userId);
    }
}
