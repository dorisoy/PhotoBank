using System;
using System.Runtime.Serialization;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Utils
{
    public static class MessageSerialization
    {
        public static ReadOnlyMemory<byte> ToBytes(Message message)
        {
            return BinarySerialization.ToBytes(message);
        }

        public static Message FromBytes(string messageTypeName, ReadOnlyMemory<byte> bytes)
        {
            try
            {
                return (Message)BinarySerialization.FromBytes(messageTypeName, bytes);
            }
            catch (SerializationException e)
            {
                return null;
            }
        }
    }
}
