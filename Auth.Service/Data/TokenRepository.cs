using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public class TokenRepository : ITokenRepository
    {
        private AuthServiceDBContext _context;

        public TokenRepository(AuthServiceDBContext context)
        {
            _context = context;
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
    }
}
