using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoRequest : AuthenticatedRequest
    {
        public int PhotoId { get; set; }
    }
}
