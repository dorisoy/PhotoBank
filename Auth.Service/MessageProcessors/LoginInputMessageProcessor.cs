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
            _context.Logger.Info(inputMessage, "Авторизация пользователя.");
            LoginOutputMessage outputMessage;
            var user = _context.RepositoryFactory.Get<IUserRepository>().GetUser(inputMessage.Login, inputMessage.Password);
            if (user != null)
            {
                var token = TokenGenerator.GetNewToken();
                var tokenPoco = new TokenPoco { Login = inputMessage.Login, UserId = user.Id, Token = token };
                _context.RepositoryFactory.Get<ITokenRepository>().AddToken(tokenPoco);
                _context.Logger.Info(inputMessage, "Создан новый токен.");
                outputMessage = new LoginOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
                {
                    Login = inputMessage.Login,
                    Token = token,
                    UserId = user.Id
                };
                _context.Logger.Info(inputMessage, "Пользователь авторизирован.");
            }
            else
            {
                outputMessage = new LoginOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
                _context.Logger.Warning(inputMessage, "Пользователь не найден.");
            }
            _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
