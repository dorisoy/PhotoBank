using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Photo.Service.Data
{
    public interface IAlbumRepository : IRepository
    {
        IEnumerable<AlbumPoco> GetUserAlbums(int userId);

        void SaveAlbum(AlbumPoco album);

        void SaveAlbums(IEnumerable<AlbumPoco> albums);

        void DeleteAlbums(IEnumerable<int> albumsId, int userId);
    }
}
