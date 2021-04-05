using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoAlbumPoco
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }

        public int AlbumId { get; set; }

        public int UserId { get; set; }
    }
}
