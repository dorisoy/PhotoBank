using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public int PhotoId { get; set; }
    }
}
