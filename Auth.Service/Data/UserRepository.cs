using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

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

        public UserPoco GetUser(string login, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
        }
    }
}
