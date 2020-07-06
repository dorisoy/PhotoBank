using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public class UserRepository : IUserRepository
    {
        private AuthServiceDBContext _context;

        public UserRepository(AuthServiceDBContext context)
        {
            _context = context;
        }

        public void AddUser(UserPoco user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
