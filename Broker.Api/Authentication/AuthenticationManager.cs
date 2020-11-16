using System.Collections.Generic;
using System.Linq;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

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

        public AuthenticationManager(IQueueManager queueManager)
        {
            queueManager.AddMessageConsumer(BrokerSettings.ResultQueue, OnLoginOutputMessage);
        }

        private void OnLoginOutputMessage(Message message)
        {
            var loginOutputMessage = message as LoginOutputMessage;
            if (loginOutputMessage != null)
            {
                this.Add(loginOutputMessage.Login, loginOutputMessage.Token, loginOutputMessage.UserId);
            }
        }

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
