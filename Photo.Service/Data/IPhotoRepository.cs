using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Photo.Service.Data
{
    public interface IPhotoRepository : IRepository
    {
        IEnumerable<PhotoPoco> GetUserPhotos(int userId);

        PhotoPoco GetPhoto(int photoId);
    }
}
