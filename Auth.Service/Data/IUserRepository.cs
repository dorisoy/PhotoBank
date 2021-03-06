using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Auth.Service.Data
{
    public interface IUserRepository : IRepository
    {
        void AddUser(UserPoco user);

        UserPoco GetUser(string login, string password);
    }
}
