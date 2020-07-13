using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Auth.Service.Data
{
    public interface ITokenRepository : IRepository
    {
        TokenPoco GetToken(string login, string token);

        void AddToken(TokenPoco token);
    }
}
