using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private Dictionary<Type, IRepository> _repositories;

        public RepositoryFactory()
        {
            _repositories = new Dictionary<Type, IRepository>();
        }

        public void Add(Type repositoryType, IRepository repositoryInstance)
        {
            if (_repositories.ContainsKey(repositoryType)) throw new RepositoryFactoryException("This repository already exists in RepositoryFactory");
            _repositories.Add(repositoryType, repositoryInstance);
        }

        public TRepository Get<TRepository>() where TRepository : IRepository
        {
            if (_repositories.ContainsKey(typeof(TRepository)))
            {
                return (TRepository)_repositories[typeof(TRepository)];
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
