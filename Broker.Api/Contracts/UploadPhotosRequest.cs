using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PhotoBank.Broker.Api.Contracts
{
    public class UploadPhotosRequest
    {
        public string Login { get; set; }

        public string Token { get; set; }

        public IEnumerable<string> Files { get; set; }
    }
}
