using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(CreateUserInputMessage))]
    public class CreateUserInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<CreateUserInputMessage>();
            var user = new UserPoco
            {
                Login = inputMessage.Login,
                Password = inputMessage.Password,
                Name = inputMessage.Name,
                EMail = inputMessage.EMail
            };
            _context.RepositoryFactory.Get<IUserRepository>().AddUser(user);
            var outputMessage = new CreateUserOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success);
            _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
