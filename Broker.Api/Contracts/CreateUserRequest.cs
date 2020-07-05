using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class CreateUserRequest
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string EMail { get; set; }
    }
}
