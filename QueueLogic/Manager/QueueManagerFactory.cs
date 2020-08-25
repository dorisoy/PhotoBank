using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Utils;

namespace PhotoBank.QueueLogic.Manager
{
    public class QueueManagerFactory
    {
        public IQueueManager Make()
        {
            var queueManagerTypeName = QueueLogicConfig.QueueManagerTypeName;
            if (queueManagerTypeName == null) throw new QueueManagerFactoryException("Не задана настройка 'queueManagerTypeName'");
            var queueManagerType = Type.GetType(queueManagerTypeName);
            var queueManagerInstance = Activator.CreateInstance(queueManagerType) as IQueueManager;
            if (queueManagerInstance == null) throw new QueueManagerFactoryException("Настройка 'queueManagerTypeName' задана неверно");

            return queueManagerInstance;
        }
    }

    public class QueueManagerFactoryException : Exception
    {
        public QueueManagerFactoryException(string message) : base(message)
        {
        }
    }
}
