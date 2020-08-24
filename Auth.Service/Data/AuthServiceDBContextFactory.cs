using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public class AuthServiceDBContextFactory
    {
        private readonly string _connectionString;

        public AuthServiceDBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AuthServiceDBContext Make()
        {
            return new AuthServiceDBContext(_connectionString);
        }
    }
}
