using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PhotoBank.DataAccess;
using PhotoBank.Photo.Contracts;

namespace PhotoBank.Photo.Service.Data
{
    public class ObsoletePhotoRemover
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ObsoletePhotoRemover(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public void Remove()
        {
            var allPhotosFromDatabase = _repositoryFactory.Get<IPhotoRepository>().GetAllPhotos().Select(photoPath => Path.Combine(PhotoSettings.RootPhotoPath, photoPath)).ToHashSet();
            var allPhotosFromDisk = Directory.GetFiles(PhotoSettings.RootPhotoPath).ToList();
            foreach (var photoFromDisk in allPhotosFromDisk)
            {
                if (allPhotosFromDatabase.Contains(photoFromDisk) == false)
                {
                    File.Delete(photoFromDisk);
                }
            }
        }
    }
}
