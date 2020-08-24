using System.Linq;

namespace PhotoBank.Auth.Service.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthServiceDBContext _context;

        public UserRepository(AuthServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
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

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
