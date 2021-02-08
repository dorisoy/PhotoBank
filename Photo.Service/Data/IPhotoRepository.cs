using System.Collections.Generic;
using PhotoBank.DataAccess;

namespace PhotoBank.Photo.Service.Data
{
    public interface IPhotoRepository : IRepository
    {
        IEnumerable<PhotoPoco> GetUserPhotos(int userId);

        PhotoPoco GetPhoto(int photoId);

        int SavePhoto(int userId, string path);

        void DeletePhoto(PhotoPoco photo);
    }
}
