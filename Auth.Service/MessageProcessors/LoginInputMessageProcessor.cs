using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(LoginInputMessage))]
    public class LoginInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<LoginInputMessage>();
            var user = _context.RepositoryFactory.Get<IUserRepository>().GetUser(inputMessage.Login, inputMessage.Password);
            var messageResult = user != null ? OutputMessageResult.Success : OutputMessageResult.Error;
            var userId = user != null ? user.Id : 0;
            var token = TokenGenerator.GetNewToken();
            var tokenPoco = new TokenPoco { Login = inputMessage.Login, UserId = userId, Token = token };
            _context.RepositoryFactory.Get<ITokenRepository>().AddToken(tokenPoco);
            var outputMessage = new LoginOutputMessage(inputMessage.ClientId, inputMessage.ChainId, messageResult) { Token = token, UserId = userId };
            _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
