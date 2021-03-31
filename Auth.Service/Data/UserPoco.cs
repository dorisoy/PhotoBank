using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public class UserPoco
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string EMail { get; set; }

        public string About { get; set; }

        public string Picture { get; set; }
    }
}
