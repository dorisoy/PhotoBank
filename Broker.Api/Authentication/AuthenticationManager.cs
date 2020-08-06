using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PhotoBank.Broker.Api.Authentication
{
    public interface IAuthenticationManager
    {
        void Add(string login, string token, int userId);

        int GetUserId(string login, string token);

        bool IsAuthenticated(string login, string token);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private List<UserAuthenticationInfo> _userAuthenticationInfo = new List<UserAuthenticationInfo>();

        public void Add(string login, string token, int userId)
        {
            _userAuthenticationInfo.RemoveAll(x => x.Login == login);
            _userAuthenticationInfo.Add(new UserAuthenticationInfo { Login = login, Token = token, UserId = userId });
        }

        public int GetUserId(string login, string token)
        {
            var userInfo = _userAuthenticationInfo.FirstOrDefault(x => x.Login == login && x.Token == token);
            return userInfo != null ? userInfo.UserId : 0;
        }

        public bool IsAuthenticated(string login, string token)
        {
            return _userAuthenticationInfo.Any(x => x.Login == login && x.Token == token);
        }

        public class UserAuthenticationInfo
        {
            public string Login { get; set; }
            public string Token { get; set; }
            public int UserId { get; set; }
        }
    }
}
