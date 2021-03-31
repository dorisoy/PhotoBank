using System;
using System.IO;
using System.Linq;
using PhotoBank.Auth.Contracts;
using PhotoBank.DataAccess;

namespace PhotoBank.Auth.Service.Data
{
    public class ObsoleteUserPictureRemover
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ObsoleteUserPictureRemover(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public void Remove()
        {
            var allUsersPicturesFromDatabase = _repositoryFactory.Get<IUserRepository>().GetAllUsersPictures().Select(path => Path.Combine(AuthSettings.RootUserPictures, path)).ToHashSet();
            var allUsersPicturesFromDisk = Directory.GetFiles(AuthSettings.RootUserPictures).ToList();
            foreach (var picturesFromDisk in allUsersPicturesFromDisk)
            {
                if (allUsersPicturesFromDatabase.Contains(picturesFromDisk) == false)
                {
                    File.Delete(picturesFromDisk);
                }
            }
        }
    }
}
