using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Photo.Service.Data
{
    public class PhotoPoco
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public byte[] Image { get; set; }
    }
}
