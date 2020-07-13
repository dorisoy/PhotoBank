using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public class TokenPoco
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
