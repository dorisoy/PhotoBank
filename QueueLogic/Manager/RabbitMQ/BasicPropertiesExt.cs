using System.Text;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    static class BasicPropertiesExt
    {
        public static string GetHeaderValue(this IBasicProperties props, string headerName)
        {
            return Encoding.UTF8.GetString((byte[])props.Headers[headerName]);
        }
    }
}
