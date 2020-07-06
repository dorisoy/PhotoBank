using System;
using PhotoBank.QueueLogic.Manager;

namespace Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueManager = new QueueManager();
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            queueManager.Send("testQueue", new TestMessage(guid1) { Message = "Message 1" });
            queueManager.Send("testQueue", new TestMessage(guid2) { Message = "Message 20" });
            var message = queueManager.WaitFor<TestMessage>("testQueue", guid2);
        }
    }

    [Serializable]
    class TestMessage : PhotoBank.QueueLogic.Contracts.InputMessage
    {
        public TestMessage(string guid) : base(guid)
        {
        }

        public string Message { get; set; }
    }
}
