using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class UploadPhotosReponse
    {
        public UploadPhotoResult Result { get; set; }

        public IEnumerable<int> PhotoIds { get; set; }
    }

    public enum UploadPhotoResult
    {
        AllPhotos = 1,
        Partially = 2,
        NoOne = 2
    }
}
