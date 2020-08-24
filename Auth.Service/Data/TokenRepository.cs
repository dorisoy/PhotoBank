using System.Linq;

namespace PhotoBank.Auth.Service.Data
{
    public class TokenRepository : ITokenRepository
    {
        private AuthServiceDBContext _context;

        public TokenRepository(AuthServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
        }

        public TokenPoco GetToken(string login, string token)
        {
            return _context.Tokens.FirstOrDefault(x => x.Login == login && x.Token == token);
        }

        public void AddToken(TokenPoco token)
        {
            var existToken = _context.Tokens.FirstOrDefault(x => x.Login == token.Login);
            if (existToken != null)
            {
                existToken.Token = token.Token;
            }
            else
            {
                _context.Tokens.Add(token);
            }
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
