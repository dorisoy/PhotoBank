using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class AuthenticatedRequest
    {
        public string Login { get; set; }

        public string Token { get; set; }
    }
}
