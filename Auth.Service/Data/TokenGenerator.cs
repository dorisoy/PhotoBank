using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PhotoBank.Auth.Service.Data
{
    public static class TokenGenerator
    {
        public static string GetNewToken()
        {
            var nowDateTime = DateTime.Now.ToString();
            var token = GetHash(nowDateTime);
            return token;
        }

        private static string GetHash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
