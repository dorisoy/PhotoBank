using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PhotoBank.Broker.Api.Contracts
{
    public class UploadPhotoRequest
    {
        public string Login { get; set; }

        public string Token { get; set; }

        public string[] Files { get; set; }
    }
}
