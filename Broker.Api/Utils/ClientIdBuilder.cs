using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Broker.Api.Utils
{
    public static class ClientIdBuilder
    {
        public static MessageClientId Build(HttpContext httpContext)
        {
            return new MessageClientId(httpContext.Connection.Id);
        }
    }
}
