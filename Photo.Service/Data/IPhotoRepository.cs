using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Photo.Service.Data
{
    public interface IPhotoRepository : IRepository
    {
        IEnumerable<PhotoPoco> GetPhotos(int userId);
    }
}
