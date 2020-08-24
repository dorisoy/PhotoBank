using System;
using System.Collections.Generic;

namespace PhotoBank.DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private Dictionary<Type, Func<IRepository>> _factoryFuncs;

        public RepositoryFactory()
        {
            _factoryFuncs = new Dictionary<Type, Func<IRepository>>();
        }

        public void Add(Type repositoryType, Func<IRepository> repositoryFactoryFunc)
        {
            if (_factoryFuncs.ContainsKey(repositoryType))
            {
                throw new RepositoryFactoryException("This repository factory function is already exist");
            }
            _factoryFuncs.Add(repositoryType, repositoryFactoryFunc);
        }

        public TRepository Get<TRepository>() where TRepository : IRepository
        {
            var repositoryType = typeof(TRepository);
            if (_factoryFuncs.ContainsKey(repositoryType))
            {
                var factoryFunc = _factoryFuncs[repositoryType];
                return (TRepository)factoryFunc();
            }
            else
            {
                return default(TRepository);
            }
        }
    }

    public class RepositoryFactoryException : Exception
    {
        public RepositoryFactoryException(string message) : base(message)
        {
        }
    }
}
